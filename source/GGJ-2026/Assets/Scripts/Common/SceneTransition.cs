using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float Duration = 1.0f;

    [Header("Animation Components")]
    [SerializeField] private Image TransitionImage;
    [SerializeField] private float TransitionImageLeftPosition;
    [SerializeField] private float TransitionImageMiddlePosition;
    [SerializeField] private float TransitionImageRightPosition;
    public static SceneTransition Instance { get; private set; }

    private Coroutine AnimationCoroutine { get; set; } = null;
    private string TargetSceneName { get; set; } = "";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        TargetSceneName = sceneName;

        if(AnimationCoroutine == null)
        {
            AnimationCoroutine = StartCoroutine(TransitionOutAnimation());
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(TargetSceneName.Length <= 0)
        {
            return;
        }

        if(AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }

        TargetSceneName = "";
        AnimationCoroutine = StartCoroutine(TransitionInAnimation());
    }

    private IEnumerator TransitionOutAnimation()
    {
        float currentDuration = 0.0f;
        Vector2 position = TransitionImage.rectTransform.anchoredPosition;

        while(currentDuration < Duration)
        {
            yield return null;
            currentDuration += Time.deltaTime;

            position.x = Mathf.Lerp(TransitionImageLeftPosition, TransitionImageMiddlePosition, currentDuration / Duration);
            TransitionImage.rectTransform.anchoredPosition = position;
        }

        position.x = TransitionImageMiddlePosition;
        TransitionImage.rectTransform.anchoredPosition = position;

        SceneManager.LoadScene(TargetSceneName);
        AnimationCoroutine = null;
    }

    private IEnumerator TransitionInAnimation()
    {
        float currentDuration = 0.0f;
        Vector2 position = TransitionImage.rectTransform.anchoredPosition;

        while (currentDuration < Duration)
        {
            yield return null;
            currentDuration += Time.deltaTime;

            position.x = Mathf.Lerp(TransitionImageMiddlePosition, TransitionImageRightPosition, currentDuration / Duration);
            TransitionImage.rectTransform.anchoredPosition = position;
        }

        position.x = TransitionImageRightPosition;
        TransitionImage.rectTransform.anchoredPosition = position;

        AnimationCoroutine = null;
    }
}
