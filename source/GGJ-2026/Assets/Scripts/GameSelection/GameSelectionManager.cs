using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSelectionManager : MonoBehaviour
{
    private Queue<GameState> MinigameQueue = new Queue<GameState>();
    private bool IsEndingSequence = false;

    private void Awake()
    {
        GameStateEnteredEvent.AddListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.AddListener(HandleGameStateExitedEvent);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStateEnteredEvent.RemoveListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.RemoveListener( HandleGameStateExitedEvent);
    }

    private void HandleGameStateEnteredEvent(GameStateEnteredEvent ev)
    {
        if(ev.EnteredState != GameState.GameSelection)
        {
            return;
        }

        if(ev.IsEnding != IsEndingSequence)
        {
            if(IsEndingSequence)
            {
                AddMinigamesToQueue();
            }
            else
            {
                MinigameQueue.Clear();
                foreach(GameState state in GameController.Instance.ScriptedEndingMiniGames)
                {
                    MinigameQueue.Enqueue(state);
                }
            }
            IsEndingSequence = ev.IsEnding;
        }

        if(MinigameQueue.Count <= 0 && !IsEndingSequence)
        {
            AddMinigamesToQueue();
        }

        gameObject.SetActive(true);

        GoToNextMinigame();
    }

    private void HandleGameStateExitedEvent(GameStateExitedEvent ev)
    {
        if(ev.ExitedState != GameState.GameSelection)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    private void AddMinigamesToQueue()
    {
        MinigameQueue.Clear();

        List<GameState> allMinigames = new List<GameState>();
        for(GameState i = GameState.GameSelection + 1; i < GameState.Count; ++i)
        {
            allMinigames.Add(i);
        }

        while(allMinigames.Count > 0)
        {
            int randomIndex = Random.Range(0, allMinigames.Count);
            MinigameQueue.Enqueue(allMinigames[randomIndex]);
            allMinigames.RemoveAt(randomIndex);
        }
    }

    private void GoToNextMinigame()
    {
        if(IsEndingSequence && MinigameQueue.Count <= 0)
        {
            GameController.Instance.SetState(GameState.WinScreen, 0.0f);
            return;
        }
        GameController.Instance.SetState(MinigameQueue.Dequeue(), 5.0f);
    }
}
