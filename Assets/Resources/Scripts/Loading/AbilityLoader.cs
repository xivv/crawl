
using System;
using System.IO;
using UnityEngine;

public class AbilityLoader : Loadable<Ability>
{

    public static AbilityLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Abilities/");

    public static Ability Get(int id)
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
            AbilityWrapper abilityWrapper = JsonUtility.FromJson<AbilityWrapper>(json); ;
            Ability ability = abilityWrapper;

            try
            {
                loaded.Add(ability.id, ability);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Ability " + ability.name + " found duplicated! Check the folder in Assets/Resources/Scripts/Abilities/Data");
            }
        }

        SetLoaded();
    }
}
