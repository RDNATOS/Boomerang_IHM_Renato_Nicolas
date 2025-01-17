using UnityEngine;
using UnityEngine.UI;

public class File
{
    public string Name { get; private set; } 
    public Text FileNameText; // ref to the UI Text component for displaying the file name


    public File(string name)
    {
        Name = name;
    }

    public void Initialize(string name)
    {
        Name = name;

        // Update the UI Text component if it exists
        if (FileNameText != null)
        {
            FileNameText.text = Name; // Set the file's name in the UI
        }
        else
        {
            Debug.LogWarning("FileNameText is not assigned for this file prefab.");
        }
    }
}
