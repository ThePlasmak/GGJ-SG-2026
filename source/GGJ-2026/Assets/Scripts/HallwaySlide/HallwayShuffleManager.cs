using UnityEngine;

public class HallwayShuffleManager : MonoBehaviour
{
    [SerializeField] private float gameDuration = 10f;
    public float GameDuration => gameDuration; // Exposed for Opponent
    private float timer;
    private bool isGameActive = false;

    private void Awake()
    {
        GameStateEnteredEvent.AddListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.AddListener(HandleGameStateExitedEvent);
    }

    private void OnDestroy()
    {
        GameStateEnteredEvent.RemoveListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.RemoveListener(HandleGameStateExitedEvent);
    }


    private System.Collections.IEnumerator TriggerDashSequence()
    {
        Debug.Log("Time's up! Dashing...");
        
        // Find Player and Opponent
        FirstPersonController player = FindObjectOfType<FirstPersonController>();
        HallwayOpponent opponent = FindObjectOfType<HallwayOpponent>();
        
        bool collisionLikely = false;

        if (player != null && opponent != null)
        {
            // 1. Disable Physics Collisions so they pass through each other
            Collider playerCol = player.GetComponent<Collider>();
            Collider opponentCol = opponent.GetComponent<Collider>();
            
            // Brutal approach: Turn off colliders completely to ensure NO physics stops them
            if (playerCol != null) playerCol.enabled = false;
            if (opponentCol != null) opponentCol.enabled = false;

            // 2. Check alignment (Are they in same lane?)
            float diffX = Mathf.Abs(player.transform.position.x - opponent.transform.position.x);
            Debug.Log($"Dash Check - PlayerX: {player.transform.position.x}, OppX: {opponent.transform.position.x}, Diff: {diffX}");
            
            collisionLikely = diffX < 2.0f; // Threshold for "Same Lane"

            // 3. Trigger Dashes (High Speed for "Boom" effect)
            player.DashForward(5f); 
            opponent.DashForward(20f); 
        }
        
        // Wait for the action to happen
        yield return new WaitForSeconds(1.5f);
        
        // Determine result based on alignment check result
        // If they were aligned, they *would have* collided (Loss)
        // If they were NOT aligned, they passed safely (Win)
        bool isWin = !collisionLikely;
        
        EndGame(isWin); 
    }

    private void HandleGameStateEnteredEvent(GameStateEnteredEvent ev)
    {
        if (ev.EnteredState == GameState.HallwaySlide)
        {
            StartGame();
        }
    }

    private void HandleGameStateExitedEvent(GameStateExitedEvent ev)
    {
        if (ev.ExitedState == GameState.HallwaySlide)
        {
            StopGame();
        }
    }

    private void StopGame()
    {
        isGameActive = false;
    }

    [Header("Camera Settings")]
    [SerializeField] private Vector3 fpsCameraStartRotation = new Vector3(0, 45f, 0); // Looking Diagonal
    [SerializeField] private Vector3 fpsCameraEndRotation = Vector3.zero; // Center/Forward

    private void StartGame()
    {
        isGameActive = true;
        timer = gameDuration;
        
        // Camera Setup: Start in FPS
        // Camera Setup: Start in FPS
        /* Camera Switcher logic removed - FPS Enforced Globally
        if (CameraSwitcher.Instance != null)
        {
            CameraSwitcher.Instance.SetFPSActive(true);
            lastSegmentIndex = -1;
        }
        */

        // Rotate FPS Camera Animation
        FirstPersonController player = FindObjectOfType<FirstPersonController>();
        if (player != null)
        {
            Camera fpsCam = player.GetComponentInChildren<Camera>();
            if (fpsCam != null)
            {
                // Start Coroutine to animate from StartRotation to EndRotation
                // Duration is the length of the first segment (1/5th of game)
                float segmentDuration = gameDuration / 5.0f;
                StartCoroutine(AnimateCameraRotation(fpsCam.transform, fpsCameraStartRotation, fpsCameraEndRotation, segmentDuration));
            }
        }
        
        Debug.Log("Hallway Shuffle Started!");
    }

    private System.Collections.IEnumerator AnimateCameraRotation(Transform camTransform, Vector3 startEuler, Vector3 endEuler, float duration)
    {
        float elapsed = 0f;
        Quaternion startRot = Quaternion.Euler(startEuler);
        Quaternion endRot = Quaternion.Euler(endEuler);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Use smooth step for nicer easing
            t = Mathf.SmoothStep(0f, 1f, t);
            
            camTransform.localRotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }
        camTransform.localRotation = endRot;
    }

    private void Start()
    {
        // Debug/Fallback: 
        // 1. If GameController is missing (Isolated test) -> Start
        // 2. If we are in "HallwaySlide" scene directly (Debug test) -> Start
        //    (Even if GameController exists and sets state to Menu, we want to play this minigame)
        
        bool isDebugScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "HallwaySlide";
        bool noController = FindObjectOfType<GameController>() == null;

        if (isDebugScene || noController)
        {
            Debug.Log($"Auto-Starting Hallway Shuffle! (DebugScene: {isDebugScene}, NoController: {noController})");
            StartGame();
        }
    }
    
    // Auto-Toggle State
    private int lastSegmentIndex = -1;

    private void Update()
    {
        if (!isGameActive) return;

        timer -= Time.deltaTime;
        
        // Auto-Toggle Camera every 1/5th Duration
        // Segment 0 (0-20%): FPS (Even)
        // Segment 1 (20-40%): TopDown (Odd)
        // Segment 2 (40-60%): FPS
        // Segment 3 (60-80%): TopDown
        // Segment 4 (80-100%): FPS
        float interval = gameDuration / 5.0f;
        float timePassed = gameDuration - timer;
        int currentSegment = Mathf.FloorToInt(timePassed / interval);
        
        // Clamp for safety
        if (currentSegment < 0) currentSegment = 0;
        
        if (currentSegment != lastSegmentIndex)
        {
            // Start with FPS (Segment 0), then alternate
            bool showFps = (currentSegment % 2 == 0);
            
            /* Camera Switcher logic removed - FPS Enforced Globally
            if (CameraSwitcher.Instance != null)
            {
                CameraSwitcher.Instance.SetFPSActive(showFps);
                Debug.Log($"Time Segment {currentSegment}: Switched to {(showFps ? "FPS" : "TopDown")}");
            }
            */
            lastSegmentIndex = currentSegment;
            lastSegmentIndex = currentSegment;
        }

        if (timer <= 0)
        {
            StartCoroutine(TriggerDashSequence());
            isGameActive = false; // Stop timer, but don't end game yet
        }
    }
    
    // Visual Feedback Variables
    private string feedbackText = "";
    private Color feedbackColor = Color.white;

    private void OnGUI()
    {
        // Debug GUI Removed as requested
    }

    public void EndGame(bool isWin)
    {
        Debug.Log($"Hallway Shuffle Ended. Win: {isWin}");
        
        // Visual Feedback
        feedbackText = isWin ? "PASSED! (WIN)" : "CRASHED! (LOSE)";
        feedbackColor = isWin ? Color.green : Color.red;
        
        // Broadcast Result to GameController to Handle State Transition
        GameStateResultEvent resultEvent = new GameStateResultEvent(isWin);
        resultEvent.Broadcast();
        
        // Disable Game Loop
        isGameActive = false;
    }
}
