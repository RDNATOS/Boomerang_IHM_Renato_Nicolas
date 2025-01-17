using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFile : MonoBehaviour, IPointerDownHandler
{
    public File LinkedFile;

    [SerializeField] private BoomerangManager boomerang; 

    public void OnPointerDown(PointerEventData eventData)
    {
        if (LinkedFile == null)
        {
            Debug.LogWarning("No LinkedFile. Can't add to boomerang.");
            return;
        }

        boomerang.AddFile(LinkedFile);

        Destroy(gameObject);
    }
}
