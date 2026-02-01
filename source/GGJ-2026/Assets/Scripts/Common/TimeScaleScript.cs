using UnityEngine;

public class TimeScaleScript : MonoBehaviour
{
    [SerializeField] private float OnEnableTimeScale = 0.0f;
    [SerializeField] private float OnDisableTimeScale = 1.0f;

    private void OnEnable()
    {
        Time.timeScale = OnEnableTimeScale;
    }

    private void OnDisable()
    {
        Time.timeScale = OnDisableTimeScale;
    }
}