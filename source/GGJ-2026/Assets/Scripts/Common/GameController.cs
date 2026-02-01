using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float TotalHealth = 100.0f;
    [SerializeField] private float FailDamage = 10.0f;
    [SerializeField] private float SuccessHeal = 10.0f;
    [SerializeField] private UnityEvent onHealthChanged;

    [Header("Win Condition")]
    [SerializeField] private float RequiredClearedMinigamesCount = 10;

    public UnityEvent OnHealthChanged { get { return onHealthChanged; } }

    [Header("Debugging Purposes")]
    [SerializeField] private GameState DebugState;
    [SerializeField] private float DebugDuration;
    [SerializeField] private bool ForceToDebugState = false;

    public static GameController Instance { get; private set; } = null;

    private GameState CurrentState = GameState.NotInitialized;
    public int ClearedMinigamesCount { get; private set; } = 0;
    public float CurrentHealth { get; private set; } = 100.0f;
    public float CurrentHealthPercent { get { return CurrentHealth / TotalHealth; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        GameStateResultEvent.AddListener(HandleGameStateResultEvent);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SetState(GameState.StartMenu, 0.0f);
    }

    private void OnDestroy()
    {
        GameStateResultEvent.RemoveListener(HandleGameStateResultEvent);
    }

    private void Update()
    {
        if(ForceToDebugState)
        {
            SetState(DebugState, DebugDuration);
            ForceToDebugState = false;
        }
    }

    public void StartGame()
    {
        ResetStats();
        SetState(GameState.GameSelection, 0.0f);
    }
    public void LoseGame()
    {
        SetState(GameState.LoseScreen,0.0f);
    }
    public void SetState(GameState newState, float targetDuration)
    {
        if (CurrentState == newState)
        {
            return;
        }

        GameStateExitedEvent exitedEvent = new GameStateExitedEvent(CurrentState);
        exitedEvent.Broadcast();

        CurrentState = newState;

        string sceneName = GameStateMapping.GetSceneName(CurrentState);
        if (sceneName.Length > 0)
        {
            if(SceneTransition.Instance != null)
            {
                SceneTransition.Instance.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            GameStateEnteredEvent enteredEvent = new GameStateEnteredEvent(CurrentState, targetDuration);
            enteredEvent.Broadcast();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameStateEnteredEvent enteredEvent = new GameStateEnteredEvent(CurrentState, 0.0f);
        enteredEvent.Broadcast();
    }

    private void HandleGameStateResultEvent(GameStateResultEvent ev)
    {
        float healthChange = (ev.IsWin) ? SuccessHeal : -FailDamage;
        CurrentHealth = Mathf.Clamp(CurrentHealth + healthChange, 0, TotalHealth);
        OnHealthChanged.Invoke();

        if(ev.IsWin)
        {
            FindAnyObjectByType<SFXManager>().Play("Sucess");
            ++ClearedMinigamesCount;
        }
        else
        {
            FindAnyObjectByType<SFXManager>().Play("Fail");
        }
        

        if(ClearedMinigamesCount >= RequiredClearedMinigamesCount)
        {
            SetState(GameState.WinScreen, 0.0f);
        }
        else if(CurrentHealth <= 0.0f)
        {
            SetState(GameState.LoseScreen, 0.0f);
        }
        else
        {
            SetState(GameState.GameSelection, 0.0f);
        }
    }

    private void ResetStats()
    {
        ClearedMinigamesCount = 0;

        CurrentHealth = TotalHealth;
        OnHealthChanged.Invoke();
    }
}