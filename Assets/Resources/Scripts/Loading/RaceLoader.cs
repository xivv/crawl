
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RaceLoader : Loadable
{

    public static RaceLoader instance;

    public Dictionary<string, Race> loaded = new Dictionary<string, Race>();
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Scripts/Races/Data");

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Race GetRace(string name)
    {
        return instance.loaded[name];
    }

    public void Load()
    {

        FileInfo[] info = dir.GetFiles("*.json");

        foreach (var file in info)
        {
            string json = File.ReadAllText(file.FullName);
            RaceWrapper wrapper = JsonUtility.FromJson<RaceWrapper>(json);
            Race race = wrapper;

            foreach (string abilityName in wrapper.abilityNames)
            {
                Ability ability = AbilityLoader.GetAbility(abilityName);

                if (ability != null)
                {
                    race.abilities.Add(ability);
                }
                else
                {
                    Debug.LogError("Ability " + abilityName + " could not be found! Check the folder in Assets/Resources/Scripts/Abilities/Data");
                }
            }

            try
            {
                loaded.Add(race.name, race);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Race " + race.name + " found duplicated! Check the folder in Assets/Resources/Scripts/Races/Data");
            }
        }

        SetLoaded();
    }
}
