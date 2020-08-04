using System;
using System.IO;
using UnityEngine;

public class DialogLoader : Loadable<InteractableDialog>
{

    public static DialogLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Dialog");


    public static InteractableDialog Get(int id)
    {
        return instance.loaded[id];
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
                loaded.Add(interactableDialog.id, interactableDialog);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Dialog " + interactableDialog.id + " found duplicated! Check the folder in " + dir.ToString());
            }
        }

        SetLoaded();
    }
}
