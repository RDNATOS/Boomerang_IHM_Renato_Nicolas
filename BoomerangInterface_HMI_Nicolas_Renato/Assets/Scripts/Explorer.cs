using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explorer : MonoBehaviour
{
    private bool explorerOpened = false;
    [SerializeField] private GameObject explorerWindowObject;
    [SerializeField] private FolderManager folderManager;

    public void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        explorerOpened = !explorerOpened;
        explorerWindowObject.SetActive(explorerOpened);

        if (explorerOpened)
        {
            folderManager.ShowRoot();
        }
    }

}

