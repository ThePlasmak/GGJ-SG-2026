using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private float ResultPitch;
    [SerializeField] private float MinPitch;
    [SerializeField] private float MaxPitch;
    [SerializeField] private float PitchChangeSpeed = 1.0f;
    [SerializeField] private AnimationCurve HealthPercentToPitchCurve;

    private float CurrentPitch { get; set; }
    private float TargetPitch { get; set; }
    private float PitchChangeCurrentDuration { get; set; }
    private float PitchChangeTotalDuration { get; set; }

    private AudioSource AudioSource { get; set; }

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();

        GameStateEnteredEvent.AddListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.AddListener(HandleGameStateExitedEvent);
    }

    private void Start()
    {
        GameController.Instance.OnHealthChanged.AddListener(HandleHealthChanged);
        HandleHealthChanged();
    }

    private void Update()
    {
        UpdatePitch(Time.deltaTime);
    }

    private void OnDestroy()
    {
        GameStateEnteredEvent.RemoveListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.RemoveListener(HandleGameStateExitedEvent);
    }

    private void HandleGameStateEnteredEvent(GameStateEnteredEvent ev)
    {
        if(ev.EnteredState != GameState.WinScreen && ev.EnteredState != GameState.LoseScreen)
        {
            return;
        }

        SetTargetPitch(ResultPitch);
    }

    private void HandleGameStateExitedEvent(GameStateExitedEvent ev)
    {
        if (ev.ExitedState != GameState.WinScreen && ev.ExitedState != GameState.LoseScreen)
        {
            return;
        }

        SetTargetPitch(MinPitch);
    }

    private void HandleHealthChanged()
    {
        float currentHealthPercent = GameController.Instance.CurrentHealthPercent;
        float targetPitch = Mathf.Lerp(MinPitch, MaxPitch, HealthPercentToPitchCurve.Evaluate(1.0f - currentHealthPercent));
        SetTargetPitch(targetPitch);
    }

    private void SetTargetPitch(float targetPitch)
    {
        CurrentPitch = AudioSource.pitch;
        TargetPitch = targetPitch;
        PitchChangeTotalDuration = Mathf.Abs(TargetPitch - CurrentPitch) / PitchChangeSpeed;
        PitchChangeCurrentDuration = 0.0f;
    }

    private void UpdatePitch(float deltaTime)
    {
        if(PitchChangeCurrentDuration >= PitchChangeTotalDuration)
        {
            return;
        }

        PitchChangeCurrentDuration += deltaTime;
        if (PitchChangeCurrentDuration >= PitchChangeTotalDuration)
        {
            AudioSource.pitch = TargetPitch;
            return;
        }

        AudioSource.pitch = Mathf.Lerp(CurrentPitch, TargetPitch, PitchChangeCurrentDuration / PitchChangeTotalDuration);
    }
}