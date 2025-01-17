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

    private Dictionary<string, string> parentMap = new Dictionary<string, string>();
    private string currentFolderName = null;

    //boomerang effect
    private bool isDragging = false; 
    private RectTransform rectTransform; // file's rec transform
    private Vector3 offset; // difference between mouse and file position
    private Vector3 initialPosition; 

    public string GetCurrentFolderName()
    {
        return currentFolderName;
    }

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

        FolderUI folderUI = folderObject.AddComponent<FolderUI>();
        folderUI.folderName = folder.Name;

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

        // component for mouse interaction
        DraggableFile draggableFile = fileObject.AddComponent<DraggableFile>();
        draggableFile.LinkedFile = file;

        Button button = fileObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnFileClick(file.Name));
        }
    }

    private void OnFolderClick(string folderName)
    {
        if (globalFolders.TryGetValue(folderName, out Folder folder))
        {
            Debug.Log($"Folder clicked: {folderName}");

            currentFolderName = folderName;

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

                parentMap[subFolderName] = parentFolderName;

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
        currentFolderName = null;

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

    public void OnBackButtonClick()
    {
        if (string.IsNullOrEmpty(currentFolderName))
        {
            Debug.Log("Root, do nothing.");
            return;
        }

        if (!parentMap.ContainsKey(currentFolderName))
        {
            Debug.Log("OnBackButtonClick => No parent => ShowRoot().");
            ShowRoot();
        }
        else
        {
            string parentName = parentMap[currentFolderName];
            Debug.Log($"OnBackButtonClick => currentFolder = {currentFolderName}, parent = {parentName}");

            OnFolderClick(parentName);
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

    private void OnFileClick(string fileName)
    {
        // when the file is clicked
        if (globalFiles.TryGetValue(fileName, out File file))
        {
            Debug.Log($"File clicked: {file.Name}");

            OpenFile(file);
        }
        else
        {
            Debug.LogError($"File '{fileName}' not found in globalFiles.");
        }
    }

    private void OpenFile(File file)
    {
        
        Debug.Log($"Opening file: {file.Name}");
    }


    private void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        isDragging = true;

        initialPosition = rectTransform.position;

        //  screen space position (eventData.position) to world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        // offset between the world position of the file and the mouse position
        offset = initialPosition - mouseWorldPosition;

        // z-value of the offset is zero to avoid issues with 3D space
        offset.z = 0;
    }


    private void OnPointerUp()
    {
        isDragging = false;
    }

    public void AddMultipleFilesToFolder(string folderName, List<File> files)
    {
        if (globalFolders.TryGetValue(folderName, out Folder folder))
        {
            foreach (File f in files)
            {
                if (!folder.Files.Contains(f))
                {
                    folder.Files.Add(f);
                    Debug.Log($"[FolderManager] File '{f.Name}' moved into '{folderName}'.");
                }
            }
        }
        else
        {
            Debug.LogError($"[FolderManager] Folder '{folderName}' not found.");
        }
    }

    public void MoveMultipleFilesToCurrentFolder(List<File> files)
    {
        if (string.IsNullOrEmpty(currentFolderName))
        {
            Debug.LogWarning("[FolderManager] No current folder selected, can't move files.");
            return;
        }

        if (!globalFolders.TryGetValue(currentFolderName, out Folder currentFolder))
        {
            Debug.LogError($"[FolderManager] Current folder '{currentFolderName}' not found in globalFolders!");
            return;
        }

        foreach (File f in files)
        {
            foreach (var kvp in globalFolders)
            {
                Folder possibleOldFolder = kvp.Value;
                if (possibleOldFolder.Files.Contains(f))
                {
                    possibleOldFolder.Files.Remove(f);
                    break;
                }
            }

            if (!currentFolder.Files.Contains(f))
            {
                currentFolder.Files.Add(f);
            }
        }

        Debug.Log($"[FolderManager] Moved {files.Count} files into '{currentFolderName}'.");

        RefreshCurrentFolderView();
    }

    public void RefreshCurrentFolderView()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        if (globalFolders.TryGetValue(currentFolderName, out Folder folder))
        {
            foreach (var subFolder in folder.SubFolders)
            {
                CreateFolderUI(subFolder);
            }
            foreach (var file in folder.Files)
            {
                CreateFileUI(file);
            }
        }
    }

    void Start()
    {
        AddFolder("Projects");
        AddFolder("Games");
        AddFolder("Zika");

        AddFile("GlobalFile.docx");
        AddFile("Notes.docx");
        AddFile("Mathematics Calculus.docx");
        AddFile("02 10 2024.docx");
        AddFile("Rapport de Stage.docx");
        AddFile("Script for next video.docx");
        AddFile("Ideas.docx");

        AddSubFolder("Projects", "2023");
        AddSubFolder("Projects", "2024");

        AddFileToFolder("Projects", "Report.docx");
        AddFileToFolder("Zika", "Model.docx");
        AddFileToFolder("Games", "GameDesign.docx");

        DisplayFolderHierarchy();
    }

    public void RemoveFileFromAnyFolder(File fileToRemove)
    {
        bool removed = false;

        foreach (var kvp in globalFolders)
        {
            Folder possibleOldFolder = kvp.Value;
            if (possibleOldFolder.Files.Contains(fileToRemove))
            {
                possibleOldFolder.Files.Remove(fileToRemove);
                Debug.Log($"[FolderManager] Removed '{fileToRemove.Name}' from '{kvp.Key}'.");
                removed = true;
                break;
            }
        }

        if (!removed)
        {
            if (globalFiles.ContainsKey(fileToRemove.Name))
            {
                globalFiles.Remove(fileToRemove.Name);
                Debug.Log($"[FolderManager] Removed '{fileToRemove.Name}' from globalFiles (root).");
            }
        }
    }

    public void AddFileToRoot(File file)
    {
        if (!globalFiles.ContainsKey(file.Name))
            globalFiles.Add(file.Name, file);
        else
            globalFiles[file.Name] = file;
    }

    void Update()
    {
        if (isDragging)
        {
            // the file starts disappearing
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, new Vector3(1.2f, 1.2f, 1), Time.deltaTime * 10f);

            //position of the file with the mouse position + the offset
            Vector3 mousePosition = Input.mousePosition + offset;
            rectTransform.position = mousePosition;

            // the rotations follows the movement direction
            Vector3 movementDirection = (mousePosition - initialPosition).normalized;
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            rectTransform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
    }
}
