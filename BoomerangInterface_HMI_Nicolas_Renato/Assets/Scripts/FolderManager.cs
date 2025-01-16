using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FolderManager : MonoBehaviour
{
    public GameObject folderPrefab;
    public GameObject filePrefab;
    public Transform contentTransform;

    private static Dictionary<string, Folder> globalFolders = new Dictionary<string, Folder>();
    private static Dictionary<string, File> globalFiles = new Dictionary<string, File>();

    public void AddFolder(string folderName)
    {
        if (!globalFolders.ContainsKey(folderName))
        {
            GameObject folderObject = Instantiate(folderPrefab, contentTransform);
            folderObject.name = folderName;

            var textComponent = folderObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = folderName;
            }

            Button button = folderObject.GetComponent<Button>();
            if (button == null)
            {
                button = folderObject.AddComponent<Button>(); 
            }

            button.onClick.AddListener(() => OnFolderClick(folderName));

            Folder newFolder = new Folder(folderName);
            globalFolders.Add(folderName, newFolder);

            Debug.Log($"Folder '{folderName}' created.");
        }
        else
        {
            Debug.LogWarning($"Folder '{folderName}' already exists globally.");
        }
    }

    public void AddFile(string fileName)
    {
        if (!globalFiles.ContainsKey(fileName))
        {
            File newFile = new File(fileName);
            globalFiles.Add(fileName, newFile);

            GameObject fileObject = Instantiate(filePrefab, contentTransform);
            fileObject.name = fileName;

            var textComponent = fileObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = fileName;
            }

            Debug.Log($"File '{fileName}' created.");
        }
        else
        {
            Debug.LogWarning($"File '{fileName}' already exists globally.");
        }
    }

    private void OnFolderClick(string folderName)
    {
        if (globalFolders.TryGetValue(folderName, out Folder folder))
        {
            Debug.Log($"Folder clicked: {folderName}");

            foreach (Transform child in contentTransform)
            {
                Destroy(child.gameObject);
            }

            foreach (var subFolder in folder.SubFolders)
            {
                AddFolder(subFolder.Name);
            }

            foreach (var file in folder.Files)
            {
                AddFile(file.Name);
            }
        }
        else
        {
            Debug.LogError($"Folder '{folderName}' not found in globalFolders.");
        }
    }

    public void AddSubFolder(string parentFolderName, string subFolderName)
    {
        if (globalFolders.TryGetValue(parentFolderName, out Folder parentFolder))
        {
            if (!parentFolder.SubFolders.Exists(f => f.Name == subFolderName))
            {
                Folder newSubFolder = new Folder(subFolderName);
                parentFolder.AddSubFolder(newSubFolder);
                globalFolders.Add(subFolderName, newSubFolder);

                Debug.Log($"Subfolder '{subFolderName}' added to '{parentFolderName}'.");
            }
            else
            {
                Debug.LogWarning($"Subfolder '{subFolderName}' already exists in '{parentFolderName}'.");
            }
        }
        else
        {
            Debug.LogError($"Parent folder '{parentFolderName}' not found.");
        }
    }

    public void AddFileToFolder(string parentFolderName, string fileName)
    {
        if (globalFolders.TryGetValue(parentFolderName, out Folder parentFolder))
        {
            if (!parentFolder.Files.Exists(f => f.Name == fileName))
            {
                if (!globalFiles.ContainsKey(fileName))
                {
                    File newFile = new File(fileName);
                    globalFiles[fileName] = newFile;
                }

                parentFolder.AddFile(globalFiles[fileName]);

                Debug.Log($"File '{fileName}' added to '{parentFolderName}'.");
            }
            else
            {
                Debug.LogWarning($"File '{fileName}' already exists in '{parentFolderName}'.");
            }
        }
        else
        {
            Debug.LogError($"Parent folder '{parentFolderName}' not found.");
        }
    }

    public void DisplayFolderHierarchy()
    {
        foreach (var folder in globalFolders.Values)
        {
            Debug.Log($"Folder: {folder.Name}");
            foreach (var subFolder in folder.SubFolders)
            {
                Debug.Log($"  - SubFolder: {subFolder.Name}");
            }
            foreach (var file in folder.Files)
            {
                Debug.Log($"  - File: {file.Name}");
            }
        }
    }

    void Start()
    {
        AddFolder("Projects");
        AddFolder("Games");
        AddFolder("Zika");

        AddSubFolder("Projects", "2023");
        AddSubFolder("Projects", "2024");

        AddFileToFolder("Projects", "Report.docx");
        AddFileToFolder("Zika", "Model.docx");

        AddFile("GlobalFile.docx");
        AddFileToFolder("Games", "GameDesign.docx");

        DisplayFolderHierarchy();
    }
}
