using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FindABreakCharacterScript : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] private List<Sprite> DefaultSprites;
    [SerializeField] private Sprite TargetSprite;
    [SerializeField] private Image Icon;

    private Image Image = null;
    private bool IsTarget = false;

    public void Initialize(bool isTarget)
    {
        if(Image == null)
        {
            Image = GetComponent<Image>();
        }

        IsTarget = isTarget;


        Image.color = (IsTarget) ? Color.green : Color.red;
        Icon.sprite = (IsTarget) ? TargetSprite : DefaultSprites[Random.Range(0, DefaultSprites.Count)];
    }

    public void SetHighlight(bool isHighlighted)
    {
        Image.enabled = isHighlighted;
    }
}
