using UnityEngine;
using UnityEditor;

public class CameraDebug
{
    [MenuItem("Tools/Debug Camera Settings")]
    public static void LogSettings()
    {
        Camera cam = Camera.main;
        if (cam == null && Camera.allCamerasCount > 0) cam = Camera.allCameras[0];
        
        if (cam != null)
        {
            Debug.Log($"Camera Name: {cam.name}");
            Debug.Log($"Projection: {(cam.orthographic ? "Orthographic" : "Perspective")}");
            Debug.Log($"FOV: {cam.fieldOfView}");
            Debug.Log($"Position: {cam.transform.position}");
            Debug.Log($"Rotation: {cam.transform.eulerAngles}");
        }
        else
        {
            Debug.LogError("No Camera found!");
        }
    }
}
