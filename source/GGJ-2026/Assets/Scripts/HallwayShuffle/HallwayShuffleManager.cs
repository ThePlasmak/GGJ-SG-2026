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
            player.DashForward(100f); 
            opponent.DashForward(100f); 
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

    private void StartGame()
    {
        isGameActive = true;
        timer = gameDuration;
        // Reset player/opponent positions if needed
        Debug.Log("Hallway Shuffle Started!");
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

    private void Update()
    {
        if (!isGameActive) return;

        timer -= Time.deltaTime;
        
        // Debug Timer UI (Optional, view in Inspector)
        // Debug.Log($"Time: {timer}");

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
        // Simple Debug UI
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = feedbackColor;
        style.alignment = TextAnchor.MiddleCenter;
        
        float w = Screen.width;
        float h = Screen.height;
        
        if (isGameActive)
        {
            // GUI.Label(new Rect(0, h * 0.1f, w, 100), $"Time: {timer:F1}", style);
        }
        else if (!string.IsNullOrEmpty(feedbackText))
        {
            GUI.Label(new Rect(0, h * 0.4f, w, 100), feedbackText, style);
        }
    }

    public void EndGame(bool isWin)
    {
        // Double check flag to prevent multiple calls if triggered purely by time
        // But here we want to allow EndGame to be called explicitly by DashSequence
        
        Debug.Log($"Hallway Shuffle Ended. Win: {isWin}");
        
        // Visual Feedback
        feedbackText = isWin ? "PASSED! (WIN)" : "CRASHED! (LOSE)";
        feedbackColor = isWin ? Color.green : Color.red;
        
        GameStateResultEvent resultEvent = new GameStateResultEvent(isWin);
        resultEvent.Broadcast();
    }
}
