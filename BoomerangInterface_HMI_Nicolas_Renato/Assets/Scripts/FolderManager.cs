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
            Folder newFolder = new Folder(folderName);
            globalFolders.Add(folderName, newFolder);

            Debug.Log($"Folder '{folderName}' created (data only).");
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

            Debug.Log($"File '{fileName}' created (data only).");
        }
        else
        {
            Debug.LogWarning($"File '{fileName}' already exists globally.");
        }
    }

    private void CreateFolderUI(Folder folder)
    {
        GameObject folderObject = Instantiate(folderPrefab, contentTransform);
        folderObject.name = folder.Name;

        var textComponent = folderObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = folder.Name;
        }

        Button button = folderObject.GetComponent<Button>();
        if (button == null)
            button = folderObject.AddComponent<Button>();

        button.onClick.AddListener(() => OnFolderClick(folder.Name));
    }

    private void CreateFileUI(File file)
    {
        GameObject fileObject = Instantiate(filePrefab, contentTransform);
        fileObject.name = file.Name;

        var textComponent = fileObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = file.Name;
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
                CreateFolderUI(subFolder);
            }

            foreach (var file in folder.Files)
            {
                CreateFileUI(file);
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

    public void ShowRoot()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        if (globalFolders.ContainsKey("Projects"))
            CreateFolderUI(globalFolders["Projects"]);

        if (globalFolders.ContainsKey("Games"))
            CreateFolderUI(globalFolders["Games"]);

        if (globalFolders.ContainsKey("Zika"))
            CreateFolderUI(globalFolders["Zika"]);

        foreach (var kvp in globalFiles)
        {
            bool isInSomeFolder = false;
            foreach (var folderEntry in globalFolders)
            {
                Folder folder = folderEntry.Value;
                if (folder.Files.Exists(file => file.Name == kvp.Key))
                {
                    isInSomeFolder = true;
                    break;
                }
            }

            if (!isInSomeFolder)
            {
                CreateFileUI(kvp.Value);
            }
        }
    }

    public void DisplayFolderHierarchy() // Debug
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

        FileManager fileManager = GetComponent<FileManager>();
        if (fileManager != null)
        {
            var fileNames = fileManager.GetRootFileNames();

            foreach (string fileName in fileNames)
            {
                AddFile(fileName);
            }
        }

        AddSubFolder("Projects", "2023");
        AddSubFolder("Projects", "2024");

        AddFileToFolder("Projects", "Report.docx");
        AddFileToFolder("Zika", "Model.docx");

        AddFile("GlobalFile.docx");
        AddFileToFolder("Games", "GameDesign.docx");

        DisplayFolderHierarchy();
    }
}
