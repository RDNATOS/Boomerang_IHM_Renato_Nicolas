using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Find the parent canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("DraggablePanel must be a child of a Canvas.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Save the original position in case you need to reset it
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optional: Add logic when the drag starts
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        Vector2 delta = eventData.delta / canvas.scaleFactor; // canvas scaling
        rectTransform.anchoredPosition += delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Optional: Add logic when the drag ends
    }

    public void ResetPosition()
    {
        // Optional: Reset the panel to its original position
        rectTransform.anchoredPosition = originalPosition;
    }
}
