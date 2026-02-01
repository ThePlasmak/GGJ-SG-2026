using UnityEngine;

/// <summary>
/// listen to this to know when you need to start your system
/// </summary>
public class GameStateEnteredEvent : GlobalEvent<GameStateEnteredEvent>
{
    public GameState EnteredState { get; private set; }
    public bool IsEnding { get; private set; }
    public float TargetDuration { get; private set; }

    public GameStateEnteredEvent(GameState enteredState, bool isEnding, float duration)
    {
        EnteredState = enteredState;
        IsEnding = isEnding;
        TargetDuration = duration;
    }
}

/// <summary>
/// broadcast this when your system wants to trigger a transition
/// </summary>
public class GameStateResultEvent : GlobalEvent<GameStateResultEvent>
{
    public bool IsWin { get; private set; }

    public GameStateResultEvent(bool isWin)
    {
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
