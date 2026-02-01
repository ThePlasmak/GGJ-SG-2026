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

    private float currentZ;
    private float elapsedTime = 0f;

    private void Start()
    {
        // Auto-find references if missing
        if (playerController == null) playerController = FindObjectOfType<FirstPersonController>();
        if (gameManager == null) gameManager = FindObjectOfType<HallwayShuffleManager>();

        currentZ = startZ;
    }

    private bool isDashing = false;

    private void Update()
    {
        if (gameManager == null || playerController == null || isDashing) return;

        elapsedTime += Time.deltaTime;
        float totalDuration = gameManager.GameDuration;
        
        // 1. Z-Axis Tension: Move closer over time
        // Fraction of time passed (0 to 1)
        float t = Mathf.Clamp01(elapsedTime / totalDuration);
        currentZ = Mathf.Lerp(startZ, endZ, t);

        // 2. X-Axis Mirroring: Match Player's Lane
        float targetX = playerController.CurrentLaneIndex * laneWidth;
        
        // Smoothly slide to the target lane
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, targetX, reactionSpeed * Time.deltaTime);
        newPos.z = currentZ;
        
        transform.position = newPos;
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
