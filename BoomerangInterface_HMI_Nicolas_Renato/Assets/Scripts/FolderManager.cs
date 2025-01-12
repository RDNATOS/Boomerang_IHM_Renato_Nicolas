using System.Collections.Generic;
using UnityEngine;

public class FolderManager : MonoBehaviour
{
    public GameObject folderPrefab;
    public GameObject filePrefab;
    public Transform contentTransform;

    private static Dictionary<string, Folder> globalFolders = new Dictionary<string, Folder>();
    private static Dictionary<string, File> globalFiles = new Dictionary<string, File>();

    // folders managed by this FolderManager
    private List<Folder> managedFolders = new List<Folder>();

    public void AddFolder(string folderName)
    {
        // to check if the folder is unique
        if (!globalFolders.ContainsKey(folderName))
        {
            Folder newFolder = new Folder(folderName);
            globalFolders.Add(folderName, newFolder);
            managedFolders.Add(newFolder);

            //  visual representation
            GameObject folderObject = Instantiate(folderPrefab, contentTransform);
            folderObject.name = folderName;

            var textComponent = folderObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = folderName;
            }

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

            //visual representation
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

    public void AddSubFolder(string parentFolderName, string subFolderName)
    {
        // parent folder
        Folder parentFolder = globalFolders.GetValueOrDefault(parentFolderName);

        if (parentFolder != null)
        {
            // parent folder is unique?
            if (!parentFolder.SubFolders.Exists(f => f.Name == subFolderName))
            {
                Folder newSubFolder = new Folder(subFolderName);
                parentFolder.AddSubFolder(newSubFolder);
                globalFolders[subFolderName] = newSubFolder;

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
        //  parent folder
        Folder parentFolder = globalFolders.GetValueOrDefault(parentFolderName);

        if (parentFolder != null)
        {
            if (!parentFolder.Files.Exists(f => f.Name == fileName))
            {
                // creates or gets the global file
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
        foreach (var folder in managedFolders)
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

        AddFileToFolder("Projects", "Report.docx");
        AddFileToFolder("AI Project", "Model.py");

        AddFile("GlobalFile.word");
        AddFileToFolder("Games", "GameDesign.pdf");

        DisplayFolderHierarchy();
    }
}
