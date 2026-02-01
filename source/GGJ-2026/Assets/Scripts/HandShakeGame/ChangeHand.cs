using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeHand : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] Sprite thisSprite;
    [SerializeField] int index;
    
    void OnMouseOver()
    {
        hand.GetComponent<SpriteRenderer>().sprite=thisSprite;
        FindAnyObjectByType<CheckResult>().SetSelectedHand(index);
    }

}
