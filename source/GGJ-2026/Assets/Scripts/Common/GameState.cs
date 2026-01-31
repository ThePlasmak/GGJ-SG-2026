using System.Collections.Generic;

public enum GameState
{
    NotInitialized,
    StartMenu,
    GameSelection,

    // Add minigame states here
    HandShakeMatching,
    FindABreak,

    // should always be the last one
    Count
}

public static class GameStateMapping
{
    private static Dictionary<GameState, string> GameStateToSceneMapping = new Dictionary<GameState, string>()
    {
        { GameState.HandShakeMatching, "HandShakeGame" },
        { GameState.FindABreak, "FindABreak" },
    };

    public static string GetSceneName(GameState state)
    {
        if(GameStateToSceneMapping.ContainsKey(state))
        {
            return GameStateToSceneMapping[state];
        }
        return "";
    }
}