using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomerangManager : MonoBehaviour
{
    public List<File> boomerangFiles = new List<File>();

    [SerializeField] private FolderManager folderManager;
    [SerializeField] private GameObject blueCircle;

    private Button boomerangButton;

    private void Awake()
    {
        boomerangButton = GetComponent<Button>();
        if (boomerangButton != null)
        {
            boomerangButton.onClick.AddListener(OnBoomerangClick);
        }
    }

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

    private void OnBoomerangClick()
    {
        if (boomerangFiles.Count == 0)
        {
            Debug.Log("[Boomerang] No files to move.");
            return;
        }

        Debug.Log($"[Boomerang] Moving {boomerangFiles.Count} files to current folder...");

        folderManager.MoveMultipleFilesToCurrentFolder(boomerangFiles);

        boomerangFiles.Clear();
        UpdateBoomerangUI();
    }

    private void UpdateBoomerangUI()
    {
        bool hasFiles = (boomerangFiles.Count > 0);
        if (blueCircle != null)
        {
            blueCircle.SetActive(hasFiles);
        }
    }
}
