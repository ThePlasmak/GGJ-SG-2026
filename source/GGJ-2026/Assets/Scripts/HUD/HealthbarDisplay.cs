using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarDisplay : MonoBehaviour
{
    [System.Serializable]
    public struct StatusDisplaySetting
    {
        [SerializeField] private float healthPercent;
        [SerializeField] private Sprite icon;
        [SerializeField] private float shakeStrength;

        public float HealthPercent { get { return healthPercent; } }
        public float ShakeStrength { get { return shakeStrength; } }
        public Sprite Icon { get { return icon; } }
    }

    [Header("Status Settings")]
    [SerializeField] private List<StatusDisplaySetting> StatusDisplaySettings;

    [Header("Required Components")]
    [SerializeField] private Image StatusIcon;
    [SerializeField] private RectTransform HealthbarFill;

    private float CurrentHealthPercent { get; set; }
    private float ShakeStrength { get; set; }

    private void Start()
    {
        GameController.Instance.OnHealthChanged.AddListener(HandleHealthChanged);
    }

    private void Update()
    {
        Vector2 statusIconOffset = Vector2.zero;
        if(ShakeStrength > 0.0f)
        {
            statusIconOffset = Random.insideUnitCircle * ShakeStrength;
        }

        StatusIcon.rectTransform.anchoredPosition = statusIconOffset;
    }

    private void HandleHealthChanged()
    {
        CurrentHealthPercent = GameController.Instance.CurrentHealthPercent;

        foreach(StatusDisplaySetting setting in StatusDisplaySettings)
        {
            if(CurrentHealthPercent >= setting.HealthPercent)
            {
                StatusIcon.sprite = setting.Icon;
                ShakeStrength = setting.ShakeStrength;
                break;
            }
        }

        Vector2 fillPosition = HealthbarFill.anchoredPosition;
        fillPosition.y = HealthbarFill.sizeDelta.y / 2.0f * (1.0f - CurrentHealthPercent);
        HealthbarFill.anchoredPosition = fillPosition;
    }
}
