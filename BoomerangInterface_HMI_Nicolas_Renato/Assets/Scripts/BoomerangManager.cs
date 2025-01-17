using System.Collections.Generic;
using UnityEngine;

public class BoomerangManager : MonoBehaviour
{
    public List<File> boomerangFiles = new List<File>();

    [SerializeField] private FolderManager folderManager;
    [SerializeField] private GameObject blueCircle;

    private void Start()
    {
        UpdateBoomerangUI();
    }

    public void AddFile(File file)
    {
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
        if (blueCircle != null)
        {
            blueCircle.SetActive(show);
        }
    }

    private void UpdateBoomerangUI()
    {
        bool hasFiles = HasFilesInBoomerang();
        ShowCircle(hasFiles);
    }

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
}
