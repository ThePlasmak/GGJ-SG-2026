using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Debugging Purposes")]
    [SerializeField] private GameState DebugState;
    [SerializeField] private bool ForceToDebugState = false;

    private GameState CurrentState { get; set; } = GameState.NotInitialized;

    private void Awake()
    {
        GameStateResultEvent.AddListener(HandleGameStateResultEvent);
    }

    private void Start()
    {
        SetState(GameState.StartMenu);
    }

    private void Update()
    {
        if(ForceToDebugState)
        {
            SetState(DebugState);
            ForceToDebugState = false;
        }
    }

    private void SetState(GameState newState)
    {
        if(CurrentState == newState)
        {
            return;
        }

        GameStateExitedEvent exitedEvent = new GameStateExitedEvent(CurrentState);
        exitedEvent.Broadcast();

        CurrentState = newState;

        GameStateExitedEvent enteredEvent = new GameStateExitedEvent(CurrentState);
        enteredEvent.Broadcast();
    }

    private void HandleGameStateResultEvent(GameStateResultEvent ev)
    {
        // handle results here


        // then transition to next state
        SetState(ev.TargetState);
    }
}