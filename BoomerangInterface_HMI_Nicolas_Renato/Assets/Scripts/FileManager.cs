using UnityEngine;

public class FileManager : MonoBehaviour
{
    public GameObject filePrefab;
    public Transform contentTransform; 

    public void AddFile(string fileName)
    {
        GameObject newFile = Instantiate(filePrefab, contentTransform);

        newFile.name = fileName;

        var textComponent = newFile.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = fileName;
        }
    }

    void Start()
    {
        CreateFiles();
    }

    public void CreateFiles()
    {
        AddFile("Notes");
        AddFile("Mathematics Calculus");
        AddFile("02 10 2024");
        AddFile("Rapport de Stage");
        AddFile("Script for next video");
        AddFile("Ideas");

        Debug.Log("Files created");
    }
}
