using UnityEngine;
using UnityEngine.InputSystem;

public class FindABreakManager : MonoBehaviour
{
    [SerializeField] private Transform CharacterRoot;
    [SerializeField] private GameObject CharacterPrefab;
    [SerializeField] private float DurationPerCharacter = 0.5f;
    [SerializeField] private int MinTargetCharacter = 3;
    
    private float TotalDuration = 0.0f;
    private float CurrentDuration = 0.0f;

    private int TotalCharacterCount = 0;
    private int TargetCharacter = 0;

    private void Awake()
    {
        GameStateEnteredEvent.AddListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.AddListener(HandleGameStateExitedEvent);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStateEnteredEvent.RemoveListener(HandleGameStateEnteredEvent);
        GameStateExitedEvent.RemoveListener(HandleGameStateExitedEvent);
    }

    private void Update()
    {
        CurrentDuration += Time.deltaTime;
        UpdateSelectedCharacter();
        TryInterject();
    }

    private void HandleGameStateEnteredEvent(GameStateEnteredEvent ev)
    {
        if(ev.EnteredState != GameState.FindABreak)
        {
            return;
        }

        TotalDuration = ev.TargetDuration;
        CurrentDuration = 0.0f;

        GenerateCharacters();
        UpdateSelectedCharacter();

        gameObject.SetActive(true);
    }

    private void HandleGameStateExitedEvent(GameStateExitedEvent ev)
    {
        if (ev.ExitedState != GameState.FindABreak)
        {
            return;
        }
        gameObject.SetActive(false);
    }

    private void GenerateCharacters()
    {
        TotalCharacterCount = Mathf.FloorToInt(TotalDuration / DurationPerCharacter);
        TargetCharacter = Random.Range(MinTargetCharacter, TotalCharacterCount);

        for(int i = 0; i < TotalCharacterCount; ++i)
        {
            FindABreakCharacterScript character = null;
            if(i < CharacterRoot.childCount)
            {
                character = CharacterRoot.GetChild(i).GetComponent<FindABreakCharacterScript>();            
            }
            else
            {
                character = Instantiate(CharacterPrefab, CharacterRoot).GetComponent<FindABreakCharacterScript>();
            }

            character.Initialize(i == TargetCharacter);
        }

        for(int i = TotalCharacterCount; i < CharacterRoot.childCount; ++i)
        {
            Destroy(CharacterRoot.GetChild(i).gameObject);
        }
    }

    private void UpdateSelectedCharacter()
    {
        int currentCharacter = GetCurrentCharacter();
        for(int i = 0; i < CharacterRoot.childCount; ++i)
        {
            CharacterRoot.GetChild(i).GetComponent<FindABreakCharacterScript>().SetHighlight(i == currentCharacter);
        }
    }

    private void TryInterject()
    {
        if(!(Mouse.current.leftButton.wasPressedThisFrame))
        {
            return;
        }

        bool isWin = GetCurrentCharacter() == TargetCharacter;
        SendResult(isWin);
    }

    private int GetCurrentCharacter()
    {
        return Mathf.FloorToInt(CurrentDuration / DurationPerCharacter);
    }

    private void SendResult(bool isWin)
    {
        GameStateResultEvent ev = new GameStateResultEvent(isWin);
        ev.Broadcast();
    }
}
