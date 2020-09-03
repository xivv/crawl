using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeroClassLoader : Loadable<HeroClass>
{
    public static HeroClassLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/HeroClasses/");

    // Generating configuration
    private static int chanceOfMultiClass = 10;
    private static int chanceOfMultiMultiClass = 5;

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

    public static HeroClass[] GenerateHeroClasses(int level)
    {
        HeroClass[] classes = new HeroClass[level];

        int chance = Random.Range(0, 100);
        bool multiClass = chance <= chanceOfMultiClass;

        if (multiClass)
        {

            if (chance <= chanceOfMultiMultiClass)
            {
                for (var i = 0; i < level; i++)
                {
                    classes[i] = GenerateRandom();
                }
            }
            else
            {
                HeroClass[] heroClasses = new HeroClass[] { GenerateRandom(), GenerateRandom() };

                for (var i = 0; i < level; i++)
                {
                    classes[i] = heroClasses[Random.Range(0, heroClasses.Length)];
                }
            }
        }
        else
        {

            HeroClass heroClass = GenerateRandom();

            for (var i = 0; i < level; i++)
            {
                classes[i] = heroClass;
            }
        }

        return classes;
    }

    public static HeroClass GenerateRandom()
    {
        return instance.GetRandom();
    }
}
