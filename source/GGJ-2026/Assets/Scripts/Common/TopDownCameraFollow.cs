using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 15, 0);
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool lockX = true; // Lock X to keep lane illusion
    [SerializeField] private bool isStatic = true; // If true, camera doesn't follow at all (good for fixed arena view)

    [SerializeField] private bool autoFitWidth = true;
    [SerializeField] private float trackWidth = 15f; // Total width of 3 lanes

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if(cam == null && target != null) cam = Camera.main;
    }

    private void LateUpdate()
    {
        // 1. Position Logic
        if (!isStatic && target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            if (lockX) desiredPosition.x = offset.x; // Lock X center
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }

        // 2. Rotation Logic (Always Look Down)
        transform.rotation = Quaternion.Euler(90, 0, 0);

        // 3. Dynamic Fit Logic
        if (autoFitWidth && cam != null)
        {
            float w = Screen.width;
            float h = Screen.height;
            float aspect = w / h;
            
            // We want the frustum width at Offset Y to match Track Width exactly
            // Or slightly larger margin? 
            // Let's match exact for 1/3 screen check.

            if (cam.orthographic)
            {
                // Ortho Size = Height of frustum / 2
                // We know Width = Size * 2 * Aspect
                // So Size = Width / (2 * Aspect)
                cam.orthographicSize = trackWidth / (2f * aspect);
            }
            else
            {
                // Perspective
                // Frustum Height = 2 * Distance * Tan(FOV / 2)
                // Frustum Width = Frustum Height * Aspect
                // So Width = 2 * Distance * Tan(FOV / 2) * Aspect
                // We want to solve for Distance (Y Height)
                // Distance = Width / (2 * Aspect * Tan(FOV / 2))
                
                float fovRad = cam.fieldOfView * 0.5f * Mathf.Deg2Rad;
                float requiredHeight = trackWidth / (2f * aspect * Mathf.Tan(fovRad));
                
                // Update Y position instantly or smoothly? Instantly ensures fit.
                Vector3 pos = transform.position;
                pos.y = requiredHeight;
                transform.position = pos;
            }
        }
    }
}
