using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] private float laneWidth = 5f; // Distance between lanes (Matches Scale 1.5 Plane)
    [SerializeField] private float scootForce = 10f;
    [SerializeField] private float scootDamping = 5f;
    [SerializeField] private bool isDashing = false;
    
    // Auto-Run Settings
    [Header("Auto Run")]
    [SerializeField] private float startZ = -20f;
    [SerializeField] private float targetZ = 0f;
    [SerializeField] private HallwayShuffleManager gameManager;
    
    // Internal State
    private int currentLaneIndex = 0; // -1: Left, 0: Center, 1: Right
    public int CurrentLaneIndex => currentLaneIndex; // Exposed for Opponent
    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false; // Hovering physics
        rb.linearDamping = 1f; // Drag
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Prevent tunneling at high speed
    }

    private void Start()
    {
        if (gameManager == null) gameManager = FindObjectOfType<HallwayShuffleManager>();
        // Ensure starting position
        Vector3 pos = transform.position;
        pos.z = startZ;
        transform.position = pos;
    }

    private void Update()
    {
        if (isDashing) return;

        // Lane Input
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Shift Left Input Detected");
            ShiftLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Shift Right Input Detected");
            ShiftLane(1);
        }
        
        // Auto-Run Logic (Move Z towards 0 based on Game Timer if available)
        if (gameManager != null && gameManager.GameDuration > 0)
        {
            float distance = targetZ - startZ;
            float speed = distance / gameManager.GameDuration;
            
            // Move Z
            Vector3 pos = transform.position;
            pos.z += speed * Time.deltaTime;
            // Clamp so we don't overshoot before dash
            if((speed > 0 && pos.z > targetZ) || (speed < 0 && pos.z < targetZ)) 
            {
                pos.z = targetZ;
            }
            transform.position = pos;
        }
    }

    private void ShiftLane(int direction)
    {
        int newLane = currentLaneIndex + direction;
        // Clamp between -1 and 1 (3 lanes)
        if (newLane >= -1 && newLane <= 1)
        {
            currentLaneIndex = newLane;
            Debug.Log($"Lane Shifted: {currentLaneIndex}");
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        // Physics "Scoot" Logic (PD Controller to snap to lane center)
        float targetX = currentLaneIndex * laneWidth;
        float currentX = transform.position.x;
        
        float error = targetX - currentX;
        float desiredVel = error * scootForce;
        
        // Apply force to reach desired velocity (Force = Mass * Acceleration)
        // Acceleration ~ (DesiredVel - CurrentVel)
        float force = (desiredVel - rb.linearVelocity.x) * scootDamping;
        
        rb.AddForce(Vector3.right * force);
    }

    public void DashForward(float speed)
    {
        isDashing = true;
        rb.isKinematic = false; // Ensure physics is on
        rb.linearDamping = 0; // Remove drag for dash
        rb.WakeUp(); // Wake up physics
        rb.AddForce(Vector3.forward * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // Check for Finish/Win/Loss
             // Using GetComponentInParent for Prefab safety as planned
            GetComponentInParent<HallwayShuffleManager>()?.EndGame(true);
        }
    }
}
