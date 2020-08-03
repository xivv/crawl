using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogLoader : Loadable
{

    public static DialogLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Dialog");
    private Dictionary<int, InteractableDialog> loadedDialogs = new Dictionary<int, InteractableDialog>();


    public static InteractableDialog GetDialog(int id)
    {
        return instance.loadedDialogs[id];
    }

    public void Awake()
    {
        Load();
    }

    public override void Load()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        FileInfo[] info = dir.GetFiles("*.json");

        foreach (var file in info)
        {
            string json = File.ReadAllText(file.FullName);
            InteractableDialog interactableDialog = JsonUtility.FromJson<InteractableDialog>(json);

            try
            {
                loadedDialogs.Add(interactableDialog.id, interactableDialog);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Dialog " + interactableDialog.id + " found duplicated! Check the folder in " + dir.ToString());
            }
        }

        SetLoaded();
    }
}
