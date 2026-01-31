using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private bool draggingFlag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        draggingFlag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingFlag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingFlag = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isDragging()
    {
        return draggingFlag;
    }
}
