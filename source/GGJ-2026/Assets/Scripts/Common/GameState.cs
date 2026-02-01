using System.Collections.Generic;

public enum GameState
{
    NotInitialized,
    StartMenu,
    // Add more states here
    WinScreen,
    LoseScreen,
    GameSelection,

    // Add minigame states here
    
    HandShakeMatching,
    FindABreak,
    HallwaySlide,
    WordJumbler,
    // should always be the last one
    Count
}

public static class GameStateMapping
{
    private static Dictionary<GameState, string> GameStateToSceneMapping = new Dictionary<GameState, string>()
    {
        { GameState.WinScreen, "Main" },
        { GameState.LoseScreen, "Main" },
        { GameState.HandShakeMatching, "HandShakeGame" },
        { GameState.FindABreak, "FindABreak" },
        { GameState.HallwaySlide, "HallwaySlide" },
        { GameState.WordJumbler, "WordJumbler" },
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

