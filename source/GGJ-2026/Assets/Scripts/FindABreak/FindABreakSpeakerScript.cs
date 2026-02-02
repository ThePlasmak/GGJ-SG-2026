using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FindABreakSpeakerScript : MonoBehaviour
{
    public enum State
    {
        Idle,
        Success,
        Fail
    }

    [Header("Sprite Settings")]
    [SerializeField] private List<Sprite> IdleSprites;
    [SerializeField] private Sprite SuccessSprite;
    [SerializeField] private Sprite FailSprite;

    public float IdleSpriteUpdateDuration { get; set; }
    public State CurrentState { get; private set; }

    private Image Image { get; set; }
    private float CurrentIdleDuration { get; set; }
    private int CurrentIdleSpriteIndex { get; set; }

    private void Update()
    {
        if(CurrentState == State.Idle && Image != null)
        {
            CurrentIdleDuration += Time.deltaTime;
            if(CurrentIdleDuration >=  IdleSpriteUpdateDuration)
            {
                CurrentIdleSpriteIndex = (CurrentIdleSpriteIndex + 1) % IdleSprites.Count;
                Image.sprite = IdleSprites[CurrentIdleSpriteIndex];

                CurrentIdleDuration -= IdleSpriteUpdateDuration;
            }
        }    
    }

    public void SetCurrentState(State newState)
    {
        if(Image == null)
        {
            Image = GetComponent<Image>();
        }

        CurrentState = newState;
        switch(CurrentState)
        {
            case State.Success:
                Image.sprite = SuccessSprite;
                break;
            case State.Fail:
                Image.sprite = FailSprite;
                break;
            case State.Idle:
                Image.sprite = IdleSprites[0];
                CurrentIdleDuration = 0.0f;
                CurrentIdleSpriteIndex = 0;
                break;
        }
    }
}
