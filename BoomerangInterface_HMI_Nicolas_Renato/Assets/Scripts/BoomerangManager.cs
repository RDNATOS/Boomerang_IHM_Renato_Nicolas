using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomerangManager : MonoBehaviour
{
    public List<File> boomerangFiles = new List<File>();

    [SerializeField] private FolderManager folderManager;

    private Image boomerangImage;
    private Button boomerangButton;

    private void Awake()
    {
        boomerangImage = GetComponent<Image>();
        boomerangButton = GetComponent<Button>();

        if (boomerangButton != null)
        {
            boomerangButton.onClick.AddListener(OnBoomerangClick);
        }
        else
        {
            Debug.LogWarning("[BoomerangManager] No Button found on boomerang object!");
        }
    }

    private void OnBoomerangClick()
    {
        if (boomerangFiles.Count == 0)
        {
            Debug.Log("[Boomerang] No files to move.");
            return;
        }

        if (folderManager == null)
        {
            Debug.LogWarning("[Boomerang] FolderManager is not assigned.");
            return;
        }

        string currentFolderName = folderManager.GetCurrentFolderName(); 

        if (string.IsNullOrEmpty(currentFolderName))
        {
            foreach (var file in boomerangFiles)
            {
                folderManager.RemoveFileFromAnyFolder(file);

                folderManager.AddFileToRoot(file);
            }

            Debug.Log($"[Boomerang] Moved {boomerangFiles.Count} files to the root.");
            boomerangFiles.Clear();

            folderManager.ShowRoot();
        }
        else
        {
            folderManager.AddMultipleFilesToFolder(currentFolderName, boomerangFiles);

            boomerangFiles.Clear();
            
            folderManager.RefreshCurrentFolderView();
        }

        UpdateBoomerangUI();
    }

    public void AddFile(File file)
    {
        folderManager.RemoveFileFromAnyFolder(file);
        
        if (!boomerangFiles.Contains(file))
        {
            boomerangFiles.Add(file);
            Debug.Log($"[Boomerang] '{file.Name}' added to boomerang.");
            UpdateBoomerangUI();
        }
    }

    public bool HasFilesInBoomerang()
    {
        return boomerangFiles.Count > 0;
    }

    public void ShowCircle(bool show)
    {
        if (boomerangImage == null) return;

        Color c = boomerangImage.color;
        c.a = show ? 1f : 0f;
        boomerangImage.color = c;
    }

    private void UpdateBoomerangUI()
    {
        ShowCircle(HasFilesInBoomerang());
    }
    /*
    public void MoveAllFilesToFolder(string folderName)
    {
        if (folderManager == null)
        {
            Debug.LogError("[Boomerang] FolderManager is not assigned!");
            return;
        }

        if (boomerangFiles.Count == 0)
        {
            Debug.Log("[Boomerang] No files to move.");
            return;
        }

        Debug.Log($"[Boomerang] Moving {boomerangFiles.Count} files to folder '{folderName}'.");

        folderManager.AddMultipleFilesToFolder(folderName, boomerangFiles);

        boomerangFiles.Clear();
        UpdateBoomerangUI();
    }
    */
}
