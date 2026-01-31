using System.Collections.Generic;
using UnityEngine;

public class GameSelectionManager : MonoBehaviour
{
    private Queue<GameState> MinigameQueue = new Queue<GameState>();

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

        if(MinigameQueue.Count <= 0)
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
        GameController.Instance.SetState(MinigameQueue.Dequeue(), 5.0f);
    }
}
