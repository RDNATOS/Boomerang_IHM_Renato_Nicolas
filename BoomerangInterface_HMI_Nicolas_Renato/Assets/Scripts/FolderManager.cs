using UnityEngine;

public class FolderManager : MonoBehaviour
{
    public GameObject folderPrefab;
    public Transform contentTransform; 

    public void AddFolder(string folderName)
    {
        GameObject newFolder = Instantiate(folderPrefab, contentTransform);

        newFolder.name = folderName;

        var textComponent = newFolder.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = folderName;
        }
    }

    void Start()
    {
        CreateFolders();
    }

    public void CreateFolders()
    {
        AddFolder("Diary");
        AddFolder("Games");
        AddFolder("Unity");
        AddFolder("Undertale");
        AddFolder("CY Tech Projects");
        AddFolder("Visual Computing - Best Speciality");
        AddFolder("Youtube Videos");
        Debug.Log("Folders created");
    }
}
