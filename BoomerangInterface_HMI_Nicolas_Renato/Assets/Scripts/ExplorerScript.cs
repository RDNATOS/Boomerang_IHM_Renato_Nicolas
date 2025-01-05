using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ExplorerScript : MonoBehaviour
{
    private bool explorerOpened = false;
    [SerializeField] private GameObject explorerWindowObject;

    public void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        explorerOpened = !explorerOpened;
        explorerWindowObject.SetActive(explorerOpened);
    }

}

