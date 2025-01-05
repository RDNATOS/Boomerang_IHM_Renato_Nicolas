using UnityEngine;
using UnityEngine.EventSystems;

public class TitleBarScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform panelRectTransform; // the plane (explorer window)
    private Canvas canvas; // canvas
    private Vector2 dragOffset;

    private void Awake()
    {
    
        panelRectTransform = transform.parent.GetComponent<RectTransform>();
        Debug.Log("Panel Rect Transform: " + panelRectTransform.name);

        // Find the parent canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("TitleBar must be a child of a Canvas.");
        }

        if (panelRectTransform == null)
        {
            Debug.LogError("Parent panel RectTransform not found!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Started on TitleBar");

        //offset positions between mouse and panel
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localMousePosition
        );

        dragOffset = panelRectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null || panelRectTransform == null) return;

        // converts mouse position to local space position on canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRectTransform.parent as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localMousePosition
        );

        // Update the panel's anchoredPosition
        panelRectTransform.anchoredPosition = localMousePosition + dragOffset;

        Debug.Log($"Dragging. New Position: {panelRectTransform.anchoredPosition}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Ended");
        EventSystem.current.SetSelectedGameObject(null); // resets the selection for new drags
    }
}

