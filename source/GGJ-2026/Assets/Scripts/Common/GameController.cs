using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Debugging Purposes")]
    [SerializeField] private GameState DebugState;
    [SerializeField] private float DebugDuration;
    [SerializeField] private bool ForceToDebugState = false;

    private GameState CurrentState { get; set; } = GameState.NotInitialized;
    public float TotalHealth { get; private set; } = 100.0f;

    private void Awake()
    {
        GameStateResultEvent.AddListener(HandleGameStateResultEvent);
    }

    private void Start()
    {
        SetState(GameState.StartMenu, 0.0f);
    }

    private void Update()
    {
        if(ForceToDebugState)
        {
            SetState(DebugState, DebugDuration);
            ForceToDebugState = false;
        }
    }

    private void SetState(GameState newState, float targetDuration)
    {
        if (CurrentState == newState)
        {
            return;
        }

        GameStateExitedEvent exitedEvent = new GameStateExitedEvent(CurrentState);
        exitedEvent.Broadcast();

        CurrentState = newState;

        GameStateEnteredEvent enteredEvent = new GameStateEnteredEvent(CurrentState);
        enteredEvent.Broadcast();
    }

    private void HandleGameStateResultEvent(GameStateResultEvent ev)
    {
        // handle results here


        // then transition to next state
        SetState(GameState.GameSelection, 0.0f);
    }
}