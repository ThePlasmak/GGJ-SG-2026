using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FindABreakCharacterScript : MonoBehaviour
{
    private Image Image = null;
    private bool IsTarget = false;

    public void Initialize(bool isTarget)
    {
        if(Image == null)
        {
            Image = GetComponent<Image>();
        }

        IsTarget = isTarget;

        Image.color = IsTarget ? Color.red : Color.black;
    }

    public void SetHighlight(bool isHighlighted)
    {
        if(IsTarget)
        {
            Image.color = isHighlighted ? Color.green : Color.red;
        }
        else
        {
            Image.color = isHighlighted ? Color.yellow : Color.black;
        }
    }
}
