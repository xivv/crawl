using System;
using System.IO;
using UnityEngine;

public class HeroClassLoader : Loadable<HeroClass>
{
    public static HeroClassLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/HeroClasses/");

    public static HeroClass Get(int id)
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
            HeroClassWrapper heroClassWrapper = JsonUtility.FromJson<HeroClassWrapper>(json);
            HeroClass heroClass = heroClassWrapper;

            // Load abilities
            foreach (ClassProgress classProgress in heroClass.progress)
            {
                LoadAbilities(classProgress.abilityIds, classProgress.abilities);
            }

            try
            {
                loaded.Add(heroClass.id, heroClass);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Item " + heroClass.name + " found duplicated! Check the folder in Assets/Resources/Scripts/Items/Data");
            }
        }

        SetLoaded();
    }
}
