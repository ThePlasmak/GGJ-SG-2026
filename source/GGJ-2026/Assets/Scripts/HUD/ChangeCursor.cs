using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorDefault;
    [SerializeField] Texture2D cursorHold;
    void OnMouseDown()
    {
        Cursor.SetCursor(cursorHold,Vector2.zero,CursorMode.Auto);
    }
    void OMouseUp()
    {
        Cursor.SetCursor(cursorDefault,Vector2.zero,CursorMode.Auto);
    }
}
