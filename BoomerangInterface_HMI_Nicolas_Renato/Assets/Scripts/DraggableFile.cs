using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DraggableFile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public File LinkedFile;

    [SerializeField] private BoomerangManager boomerang;

    [Header("Drag Settings")]
    public float dragThreshold = 32f;

    public float rotationDuration = 0.5f;

    public float rotationSpeed = 1000f;

    private RectTransform rectTransform;
    private Vector2 startDragPosition;
    private bool hasTriggeredRotation = false;
    private bool isDragging = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (boomerang == null)
        {
            boomerang = FindObjectOfType<BoomerangManager>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        hasTriggeredRotation = false;

        startDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        rectTransform.position = eventData.position;

        float distance = Vector2.Distance(eventData.position, startDragPosition);

        if (distance > dragThreshold && !hasTriggeredRotation)
        {
            hasTriggeredRotation = true;
            StartCoroutine(RotateThenDisappear());
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private IEnumerator RotateThenDisappear()
    {
        if (LinkedFile == null)
            Debug.LogError("[DraggableFile] LinkedFile is null!");

        if (boomerang == null)
            Debug.LogError("[DraggableFile] Boomerang is null!");

        if (LinkedFile != null && boomerang != null)
        {
            boomerang.AddFile(LinkedFile);
        }

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float delta = Time.deltaTime;
            elapsedTime += delta;

            float angleThisFrame = rotationSpeed * delta;
            rectTransform.Rotate(0f, 0f, angleThisFrame);

            yield return null;
        }

        Destroy(gameObject);
    }
}
