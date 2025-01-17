using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public File LinkedFile;

    private RectTransform rectTransform;
    private Vector3 offset;
    private Vector3 initialPosition;
    private bool isDragging = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        initialPosition = rectTransform.position;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = initialPosition - mouseWorldPosition;
        offset.z = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            rectTransform.position = mouseWorldPos + offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
