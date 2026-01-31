using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float TotalHealth = 100.0f;
    [SerializeField] private float FailDamage = 10.0f;
    [SerializeField] private float SuccessHeal = 10.0f;

    [Header("Debugging Purposes")]
    [SerializeField] private GameState DebugState;
    [SerializeField] private float DebugDuration;
    [SerializeField] private bool ForceToDebugState = false;

    public static GameController Instance { get; private set; } = null;

    private GameState CurrentState { get; set; } = GameState.NotInitialized;
    public int ClearedMinigamesCount { get; private set; } = 0;
    public float CurrentHealth { get; private set; } = 100.0f;
    public float CurrentHealthPercent { get { return CurrentHealth / TotalHealth; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        GameStateResultEvent.AddListener(HandleGameStateResultEvent);
    }

    private void Start()
    {
        SetState(GameState.GameSelection, 0.0f);
    }

    private void OnDestroy()
    {
        GameStateResultEvent.RemoveListener(HandleGameStateResultEvent);
    }

    private void Update()
    {
        if(ForceToDebugState)
        {
            SetState(DebugState, DebugDuration);
            ForceToDebugState = false;
        }
    }

    public void SetState(GameState newState, float targetDuration)
    {
        if (CurrentState == newState)
        {
            return;
        }

        GameStateExitedEvent exitedEvent = new GameStateExitedEvent(CurrentState);
        exitedEvent.Broadcast();

        CurrentState = newState;

        GameStateEnteredEvent enteredEvent = new GameStateEnteredEvent(CurrentState, targetDuration);
        enteredEvent.Broadcast();
    }

    private void HandleGameStateResultEvent(GameStateResultEvent ev)
    {
        float healthChange = (ev.IsWin) ? SuccessHeal : -FailDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth + healthChange, 0, TotalHealth);

        if(ev.IsWin)
        {
            ++ClearedMinigamesCount;
        }

        SetState(GameState.GameSelection, 0.0f);
    }
}