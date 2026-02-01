using System.Collections.Generic;
using UnityEngine;

public class EnableWithGameStateScript : MonoBehaviour
{
    public enum EnableModeType
    {
        UseTarget,
        UseExcluded,
    }

    [SerializeField] private EnableModeType EnableMode;
    [SerializeField] private GameState TargetState;
    [SerializeField] private List<GameState> ExcludedList;

    private void Awake()
    {
        GameStateEnteredEvent.AddListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.AddListener(HandleGameStateExitedEvent);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStateEnteredEvent.RemoveListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.RemoveListener(HandleGameStateExitedEvent);
    }

    private void HandleGameStateEnteredEvent(GameStateEnteredEvent ev)
    {
        if(!IsValidState(ev.EnteredState))
        {
            return;
        }

        gameObject.SetActive(true);
    }

    private void HandleGameStateExitedEvent(GameStateExitedEvent ev)
    {
        if (!IsValidState(ev.ExitedState))
        {
            return;
        }

        gameObject.SetActive(false);
    }

    private bool IsValidState(GameState state)
    {
        switch(EnableMode)
        {
            case EnableModeType.UseTarget:
                return state == TargetState;
            case EnableModeType.UseExcluded:
                return !(ExcludedList.Contains(state));
            default:
                return false;
        }
    }
}
