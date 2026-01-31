using UnityEngine;

public class CentralControll : MonoBehaviour
{
    // Start is called o
    // nce before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject everything;
    [SerializeField] GameObject options;
    
    void Start()
    {
        // everything.SetActive(false);
        GameStateEnteredEvent.AddListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.AddListener(HandleGameStateExitEvent);
    }
    void OnDestroy()
    {
        GameStateEnteredEvent.RemoveListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.AddListener(HandleGameStateExitEvent);
    }


    void HandleGameStateEnteredEvent(GameStateEnteredEvent ev)
    {
        if(ev.EnteredState==GameState.HandShakeMatching){
            print("2");
            everything.SetActive(true);
            options.SetActive(true);
        }
    }
    void HandleGameStateExitEvent(GameStateExitedEvent ev)
    {
        if(ev.ExitedState==GameState.HandShakeMatching){
            print("2");
            everything.SetActive(false);
        }
    }
}
