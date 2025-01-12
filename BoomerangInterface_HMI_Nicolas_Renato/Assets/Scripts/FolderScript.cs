using System.Collections.Generic;
using UnityEngine;

public class Folder
{
    public string Name { get; private set; }
    public List<Folder> SubFolders { get; private set; }
    public List<File> Files { get; private set; }

    public Folder(string name)
    {
        Name = name;
        SubFolders = new List<Folder>();
        Files = new List<File>();
    }

    public void AddSubFolder(Folder subFolder)
    {
        if (SubFolders.Exists(folder => folder.Name == subFolder.Name))
        {
            Debug.Log($"Subfolder '{subFolder.Name}' already exists in '{Name}'");
        }
        else
        {
            SubFolders.Add(subFolder);
        }
    }

    public void AddFile(File file)
    {
        if (Files.Exists(f => f.Name == file.Name))
        {
            Debug.Log($"File '{file.Name}' already exists in '{Name}'");
        }
        else
        {
            Files.Add(file);
        }
    }
}
