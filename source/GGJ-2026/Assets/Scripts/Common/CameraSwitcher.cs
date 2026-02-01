using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera topDownCamera; // Kept to disable it explicitly
    [SerializeField] private Camera fpsCamera;
    
    // Always active
    public bool IsFpsActive { get { return true; } }
    
    public event System.Action<bool> OnFPSToggled;

    public static CameraSwitcher Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Enforce FPS Mode
        if (fpsCamera != null) fpsCamera.enabled = true;
        if (topDownCamera != null) topDownCamera.enabled = false;
        
        // Notify listeners
        OnFPSToggled?.Invoke(true);
        
        Debug.Log("Camera Logic: Fixed FPS Mode Enforced. TopDown Disabled.");
    }
}
