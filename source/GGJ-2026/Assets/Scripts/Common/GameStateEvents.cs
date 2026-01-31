using UnityEngine;

/// <summary>
/// listen to this to know when you need to start your system
/// </summary>
public class GameStateEnteredEvent : GlobalEvent<GameStateEnteredEvent>
{
    public GameState EnteredState { get; private set; }

    public GameStateEnteredEvent(GameState enteredState)
    {
        EnteredState = enteredState;
    }
}

/// <summary>
/// broadcast this when your system wants to trigger a transition
/// </summary>
public class GameStateResultEvent : GlobalEvent<GameStateResultEvent>
{
    public GameState TargetState { get; private set; }
    public bool IsWin { get; private set; }

    public GameStateResultEvent(GameState targetState, bool isWin)
    {
        TargetState = targetState;
        IsWin = isWin;
    }
}

/// <summary>
/// listen to this to know when you need to stop your system
/// </summary>
public class GameStateExitedEvent : GlobalEvent<GameStateExitedEvent>
{
    public GameState ExitedState { get; private set; }

    public GameStateExitedEvent(GameState exitedState)
    {
        ExitedState = exitedState;
    }
}
