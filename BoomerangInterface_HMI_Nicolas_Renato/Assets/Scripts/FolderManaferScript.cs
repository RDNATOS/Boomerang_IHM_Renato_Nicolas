using UnityEngine;

public class FolderManager : MonoBehaviour
{
    public GameObject folderPrefab; 
    public Transform contentTransform; //the contect object in the hierarchy

    public void AddFolder(string folderName, float posX, float posY)
    {
        // instantiation of the prefab
        GameObject newFolder = Instantiate(folderPrefab, contentTransform);

        newFolder.name = folderName;

        // rect transform to set the position
        RectTransform folderRectTransform = newFolder.GetComponent<RectTransform>();
        if (folderRectTransform != null)
        {
            folderRectTransform.anchoredPosition = new Vector2(posX, posY);
        }
        var textComponent = newFolder.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = folderName;
        }
    }



    void Start()
    {
        createFolders();
    }

    public void createFolders()
    {
        var folderManager = GetComponent<FolderManager>();
        folderManager.AddFolder("Diary", -430, -100);
        folderManager.AddFolder("Games", -230, -100);
        folderManager.AddFolder("Unity", -30, -100);
        folderManager.AddFolder("Undertale", 170, -100);
        folderManager.AddFolder("CY Tech Projects", 370, -100);
        folderManager.AddFolder("Visual Computing - Best Speciality", -430, -400);
        folderManager.AddFolder("Youtube Videos", -230, -400);
        Debug.Log("Folders created");
    }
}