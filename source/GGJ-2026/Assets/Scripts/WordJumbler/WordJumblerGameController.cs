using UnityEngine;

public class WordJumblerGameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject normalCharacter;
    public GameObject finalCharacter;

    void Start()
    {
        GameStateEnteredEvent.AddListener(HandleGameStateEnteredEvent);
        normalCharacter.SetActive(true);   
        finalCharacter.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        GameStateEnteredEvent.RemoveListener(HandleGameStateEnteredEvent);
    }

    private void HandleGameStateEnteredEvent(GameStateEnteredEvent ev)
    {
        if (ev.EnteredState != GameState.WordJumbler)
        {
            return;
        }
        normalCharacter.SetActive(false);
        finalCharacter.SetActive(true);
    }
}
