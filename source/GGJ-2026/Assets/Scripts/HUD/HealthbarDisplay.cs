using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarDisplay : MonoBehaviour
{
    [System.Serializable]
    public class StatusDisplaySetting
    {
        [SerializeField] private float minHealthPercent;
        [SerializeField] private Sprite icon;
        [SerializeField] private float minShakeStrength;
        [SerializeField] private float maxShakeStrength;
        [SerializeField] private float shakeSpeed;

        public float MinHealthPercent { get { return minHealthPercent; } }
        public float MinShakeStrength { get { return minShakeStrength; } }
        public float MaxShakeStrength { get { return maxShakeStrength; } }
        public float ShakeSpeed { get { return shakeSpeed; } }
        public Sprite Icon { get { return icon; } }
    }

    [Header("Status Settings")]
    [SerializeField] private List<StatusDisplaySetting> StatusDisplaySettings;

    [Header("Required Components")]
    [SerializeField] private Image StatusIcon;
    [SerializeField] private RectTransform HealthbarFill;

    private float CurrentHealthPercent { get; set; }
    private StatusDisplaySetting CurrentStatusDisplaySetting { get; set; } = null;

    private Vector2 PreviousStatusShakeOffset = Vector2.zero;
    private Vector2 TargetStatusShakeOffset = Vector2.zero;
    private float TargetStatusShakeTotalDuration = 0.0f;
    private float TargetStatusShakeCurrentDuration = 0.0f;

    private void Start()
    {
        GameController.Instance.OnHealthChanged.AddListener(HandleHealthChanged);
        HandleHealthChanged();
    }

    private void Update()
    {
        if (CurrentStatusDisplaySetting.MaxShakeStrength <= 0.0f
            || CurrentStatusDisplaySetting.MinShakeStrength > CurrentStatusDisplaySetting.MaxShakeStrength)
        {
            return;
        }

        TargetStatusShakeCurrentDuration += Time.deltaTime;

        if (TargetStatusShakeCurrentDuration >= TargetStatusShakeTotalDuration)
        {
            PreviousStatusShakeOffset = TargetStatusShakeOffset;
            TargetStatusShakeCurrentDuration -= TargetStatusShakeTotalDuration;

            float randomStrength = Random.Range(CurrentStatusDisplaySetting.MinShakeStrength, CurrentStatusDisplaySetting.MaxShakeStrength);
            TargetStatusShakeOffset = Random.insideUnitCircle.normalized * randomStrength;
            TargetStatusShakeTotalDuration += randomStrength / CurrentStatusDisplaySetting.ShakeSpeed;
        }

        StatusIcon.rectTransform.anchoredPosition = Vector2.Lerp(PreviousStatusShakeOffset, TargetStatusShakeOffset, 
            TargetStatusShakeCurrentDuration / TargetStatusShakeTotalDuration);
    }

    private void HandleHealthChanged()
    {
        CurrentHealthPercent = GameController.Instance.CurrentHealthPercent;

        StatusDisplaySetting newSetting = null;
        foreach (StatusDisplaySetting setting in StatusDisplaySettings)
        {
            if(CurrentHealthPercent >= setting.MinHealthPercent)
            {
                newSetting = setting;
                break;
            }
        }
        if (newSetting != CurrentStatusDisplaySetting)
        {
            CurrentStatusDisplaySetting = newSetting;
            StatusIcon.sprite = CurrentStatusDisplaySetting.Icon;
            StatusIcon.rectTransform.anchoredPosition = Vector2.zero;
            PreviousStatusShakeOffset = TargetStatusShakeOffset = Vector2.zero;
            TargetStatusShakeTotalDuration = TargetStatusShakeCurrentDuration = 0.0f;
        }

        Vector2 fillPosition = HealthbarFill.anchoredPosition;
        fillPosition.y = HealthbarFill.sizeDelta.y * (1.0f - CurrentHealthPercent);
        HealthbarFill.anchoredPosition = fillPosition;
    }
}
