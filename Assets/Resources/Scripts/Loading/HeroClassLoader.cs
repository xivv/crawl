using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HeroClassLoader : Loadable
{
    public static HeroClassLoader instance;

    public Dictionary<string, HeroClass> loadedHeroClasses = new Dictionary<string, HeroClass>();
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Scripts/HeroClasses/Data");

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static HeroClass getHeroClass(string name)
    {
        return instance.loadedHeroClasses[name];
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
                LoadAbilities(classProgress.abilityNames, classProgress.abilities);
            }

            try
            {
                loadedHeroClasses.Add(heroClass.name, heroClass);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Item " + heroClass.name + " found duplicated! Check the folder in Assets/Resources/Scripts/Items/Data");
            }
        }

        SetLoaded();
    }
}
