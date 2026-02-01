using UnityEngine;

public class HallwayOpponent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FirstPersonController playerController;
    [SerializeField] private HallwayShuffleManager gameManager;
    
    [Header("Settings")]
    [SerializeField] private float laneWidth = 5f; // Must match Player's lane width
    [SerializeField] private float reactionSpeed = 5f; // How fast to slide to new lane
    [SerializeField] private float startZ = 20f; // Adjusted for symmetrical setup
    [SerializeField] private float endZ = 0f; // Meeting in the middle

    [Header("Visuals")]
    [SerializeField] private float baseY = 0.3f; // Lift off ground
    [SerializeField] private float eyesYOffset = 0.2f; // Offset from body center
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer eyesRenderer;
    [SerializeField] private Sprite bodySprite; // For manual assignment if needed
    [SerializeField] private Sprite eyesSprite;
    
    [Header("Animation")]
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float eyeLagDrag = 0.3f; // Increased for visibility
    [SerializeField] private float eyeReturnSpeed = 10f; // Faster snap-back

    private float currentVelocityX;
    private float targetLaneX;
    private float visualY = 0f;
    private float eyesOffsetX = 0f;
    
    // Restored Logic Variables
    private float currentZ;
    private float elapsedTime = 0f;
    private bool isDashing = false;

    private void Start()
    {
        // Auto-find references if missing
        if (playerController == null) playerController = FindObjectOfType<FirstPersonController>();
        if (gameManager == null) gameManager = FindObjectOfType<HallwayShuffleManager>();

        // Disable Cube Mesh
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        currentZ = startZ;
        if(playerController != null)
             targetLaneX = playerController.CurrentLaneIndex * laneWidth;
    }

    private void Update()
    {
        if (gameManager == null) return;
        if(isDashing) return; // Stop logic if dashing
        if (playerController == null) return;

        elapsedTime += Time.deltaTime;
        float totalDuration = gameManager.GameDuration;
        
        // 1. Z-Axis Tension: Move closer over time
        float t = Mathf.Clamp01(elapsedTime / totalDuration);
        currentZ = Mathf.Lerp(startZ, endZ, t);

        // 2. X-Axis Mirroring: Match Player's Lane
        float newTargetX = playerController.CurrentLaneIndex * laneWidth;

        // Visual Bop Logic: If target changes, trigger jump state or just oscillate based on movement
        // We will trigger a 'Jump' simply by checking if we are moving fast
        
        // Smoothly slide to the target lane
        Vector3 newPos = transform.position;
        float prevX = newPos.x;
        newPos.x = Mathf.Lerp(newPos.x, newTargetX, reactionSpeed * Time.deltaTime);
        newPos.z = currentZ;
        
        // Bop Logic: Sine wave based on continuous movement? 
        // Or trigger? Let's do simple sine wave when moving laterally
        float speedX = Mathf.Abs(newPos.x - prevX) / Time.deltaTime;
        
        // If moving significantly, increase Y
        if (speedX > 0.1f)
        {
             // Simple arc: sin(time * speed)
             // We want it to be 0 when stopped. 
             // Let's just PingPong Y toward JumpHeight when moving towards target?
             // Actually, "Bop on Lane Change" is specific.
             
             if(Mathf.Abs(newTargetX - transform.position.x) > 0.5f)
             {
                 // We are traveling
                 float distRatio = 1.0f - (Mathf.Abs(newTargetX - transform.position.x) / laneWidth);
                 // Peak at center of travel (0.5), 0 at ends
                 // Parabola: 4 * x * (1-x)
                 visualY = 4 * distRatio * (1 - distRatio) * jumpHeight;
             }
             else
             {
                 visualY = Mathf.Lerp(visualY, 0, jumpSpeed * Time.deltaTime);
             }
        }
        else
        {
             visualY = Mathf.Lerp(visualY, 0, jumpSpeed * Time.deltaTime);
        }
        
        // Apply Y to Body Renderer (keep parent grounded?)
        // Assuming Hierarchy: Opponent -> BodySprite
        if (bodyRenderer != null)
        {
            Vector3 localPos = bodyRenderer.transform.localPosition;
            localPos.y = visualY + baseY;
            bodyRenderer.transform.localPosition = localPos;
        }

        // Eye Lag Logic
        float moveDelta = newPos.x - prevX;
        // Eyes move Opposite to movement (Lag)
        float targetEyeOffset = -moveDelta * eyeLagDrag * 100f; // Multiplier to make it visible
        eyesOffsetX = Mathf.Lerp(eyesOffsetX, targetEyeOffset, eyeReturnSpeed * Time.deltaTime);
        
        // Apply to Eyes
        if (eyesRenderer != null)
        {
            // Eyes should follow Body Height + Lag X + Eye Offset
            Vector3 localPos = eyesRenderer.transform.localPosition;
            localPos.y = visualY + baseY + eyesYOffset; // Sync height + Base + Head Offset
            localPos.x = eyesOffsetX; // Apply lag
            // Ensure Z is slightly front so it renders over body
            localPos.z = -0.1f; // Increased offset to ensure it sits on top definitely
            eyesRenderer.transform.localPosition = localPos;
        }

        transform.position = newPos;

        // Billboarding: Force Sprites to look at Camera
        BillboardSprites();
    }

    private void BillboardSprites()
    {
        // Fixed FPS Mode: Always face camera upright.
        // If the sprite is "lying on its back", it likely has rotation (90, 0, 0).
        // To make it stand up, we want rotation (0, 0, 0).
        // This assumes the Camera is looking forward (0,0,0) or close to it.
        // Even if the camera looks down slightly, we usually want the sprite to stay vertical (World Up).
        
        Quaternion uprightRotation = Quaternion.identity;
        
        // Optional: Face the Camera's Y-rotation only (Look at Camera on the floor plane)
        Camera cam = Camera.main;
        if(cam == null) cam = FindObjectOfType<Camera>();
        
        if (cam != null)
        {
             // Get look direction
             Vector3 dirToCam = cam.transform.position - transform.position;
             dirToCam.y = 0; // Flatten to ground
             if(dirToCam.sqrMagnitude > 0.001f)
             {
                 uprightRotation = Quaternion.LookRotation(-dirToCam); 
                 // Note: Quads usually face -Z (Back). 
                 // If we LookRotation(-dir), Forward points AWAY from camera. Back points AT camera.
                 // This is correct for Unity Quads/SpriteRenderers usually.
                 
                 // However, "Identity" (0,0,0) faces +Z (World Forward).
                 // If "Lying on back" was 90 deg X, then 0 deg X is standing.
                 // Let's stick to Identity first?
                 // No, Billboarding = Facing Camera.
                 // LookRotation(dirToCam) makes Z point at Camera.
                 // Sprite (X,Y plane) faces Z. So we want LookRotation(dirToCam).
                 
                 // Let's try matching Camera Y Rotation only.
                 Vector3 camEuler = cam.transform.eulerAngles;
                 uprightRotation = Quaternion.Euler(0, camEuler.y, 0);
             }
        }

        if(bodyRenderer != null) BillboardIndividual(bodyRenderer.transform, uprightRotation);
        if(eyesRenderer != null) BillboardIndividual(eyesRenderer.transform, uprightRotation);
    }

    private void BillboardIndividual(Transform t, Quaternion targetRot)
    {
        t.rotation = targetRot;
    }



    public void DashForward(float speed)
    {
        isDashing = true;
        // Move opposite to player (towards negative Z) or just forward relative to self?
        // Assuming Opponent is facing -Z (towards player), forward is correct. 
        // If not, we might need Vector3.back. Let's assume World Space -Z for "Passing".
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.isKinematic = false; 
            rb.linearDamping = 0;
            rb.AddForce(Vector3.back * speed, ForceMode.VelocityChange);
        }
        else
        {
            // Fallback if no rigidbody
            StartCoroutine(DashRoutine(speed));
        }
    }
    
    private System.Collections.IEnumerator DashRoutine(float speed)
    {
        float duration = 2f;
        while(duration > 0)
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }
    }
}
