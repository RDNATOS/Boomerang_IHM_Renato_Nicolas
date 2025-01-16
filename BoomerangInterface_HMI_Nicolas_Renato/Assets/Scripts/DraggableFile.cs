using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
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

        // screen space to world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        offset = initialPosition - mouseWorldPosition;

        offset.z = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

            // offset to maintain relative positioning
            rectTransform.position = mouseWorldPosition + offset;

            // the file is scalled when dragging
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, new Vector3(1.2f, 1.2f, 1), Time.deltaTime * 10f);

            // file is rotated based on the movement direction
            Vector3 movementDirection = (mouseWorldPosition + offset - initialPosition).normalized;
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            rectTransform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
    }
}
