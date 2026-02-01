using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public void Change(Sprite sprite1)
    {
        GetComponent<SpriteRenderer>().sprite=sprite1;
        // print("1");
    }
}
