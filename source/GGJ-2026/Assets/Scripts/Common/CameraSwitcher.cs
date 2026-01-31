using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera topDownCamera;
    [SerializeField] private Camera fpsCamera;
    
    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.C;
    
    private bool isFpsActive = false;

    private void Start()
    {
        // Initial State: Top-Down Active, FPS Disabled
        if (topDownCamera != null) topDownCamera.enabled = true;
        if (fpsCamera != null) fpsCamera.enabled = false;
        isFpsActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleCamera();
        }
    }

    private void ToggleCamera()
    {
        isFpsActive = !isFpsActive;
        
        if (topDownCamera != null) topDownCamera.enabled = !isFpsActive;
        if (fpsCamera != null) fpsCamera.enabled = isFpsActive;
        
        Debug.Log($"Camera Models Switched. FPS Active: {isFpsActive}");
    }
}
