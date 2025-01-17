using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoomerangManager : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<File> boomerangFiles = new List<File>();

    [SerializeField] private FolderManager folderManager;
    [SerializeField] private GameObject blueCircle;

    private RectTransform rectTransform;
    private Vector2 offset;
    private Vector2 initialAnchoredPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialAnchoredPosition = rectTransform.anchoredPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableFile draggableFile = eventData.pointerDrag?.GetComponent<DraggableFile>();
        if (draggableFile != null && draggableFile.LinkedFile != null)
        {
            File draggedFile = draggableFile.LinkedFile;
            if (!boomerangFiles.Contains(draggedFile))
            {
                boomerangFiles.Add(draggedFile);
                Debug.Log($"[Boomerang] Added file '{draggedFile.Name}' to boomerang.");
            }

            UpdateBlueCircle();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransform parentRect = rectTransform.parent as RectTransform;
        if (parentRect == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        offset = rectTransform.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform parentRect = rectTransform.parent as RectTransform;
        if (parentRect == null) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        ))
        {
            rectTransform.anchoredPosition = localPoint + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = initialAnchoredPosition;

        GameObject hoveredObject = eventData.pointerCurrentRaycast.gameObject;
        if (hoveredObject != null)
        {
            if (hoveredObject.CompareTag("ScrollView"))
            {
                MoveAllBoomerangFilesToCurrentFolder();
            }
        }
    }

    private void MoveAllBoomerangFilesToCurrentFolder()
    {
        if (boomerangFiles.Count == 0)
        {
            Debug.Log("[Boomerang] No files to move.");
            return;
        }

        Debug.Log($"[Boomerang] Moving {boomerangFiles.Count} files to current folder...");
        folderManager.MoveMultipleFilesToCurrentFolder(boomerangFiles);

        boomerangFiles.Clear();
        UpdateBlueCircle();
    }

    

    private void UpdateBlueCircle()
    {
        bool hasFiles = (boomerangFiles.Count > 0);
        if (blueCircle != null)
            blueCircle.SetActive(hasFiles);
    }
}
