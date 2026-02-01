using UnityEngine;

public class RuntimeCameraAdjuster : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotateSpeed = 45f;
    [SerializeField] private float fovSpeed = 10f;
    
    private void Update()
    {
        // Only run if we are in experimental mode or just generally available for now
        Camera cam = Camera.main;
        if (cam == null) return;
        
        // Rotation (Arrow Keys)
        float h = 0f;
        float v = 0f;
        
        if (Input.GetKey(KeyCode.UpArrow)) v = -1f;
        if (Input.GetKey(KeyCode.DownArrow)) v = 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) h = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) h = 1f;
        
        // Hold Shift to Rotate, otherwise specific adjustments
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
             // Shift + Arrows = Rotate
             Vector3 rot = cam.transform.eulerAngles;
             rot.x += v * rotateSpeed * Time.deltaTime;
             rot.y += h * rotateSpeed * Time.deltaTime;
             cam.transform.eulerAngles = rot;
        }
        else
        {
            // Just Arrows = Pitch/Yaw simpler? Or Position?
            // Nothing for now, stick to Shift+Arrows for rotation.
        }
        
        // FOV: PageUp/PageDown
        if (Input.GetKey(KeyCode.PageUp)) cam.fieldOfView -= fovSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.PageDown)) cam.fieldOfView += fovSpeed * Time.deltaTime;
        
        // Log on Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"[CameraAdjuster] Pos: {cam.transform.position}, Rot: {cam.transform.eulerAngles}, FOV: {cam.fieldOfView}");
        }
    }
}
