
using System;
using System.IO;
using UnityEngine;

public class RaceLoader : Loadable<Race>
{

    public static RaceLoader instance;

    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Races/");

    public static Race Get(int id)
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
            RaceWrapper wrapper = JsonUtility.FromJson<RaceWrapper>(json);
            Race race = wrapper;

            foreach (int abilityId in wrapper.abilityIds)
            {
                Ability ability = AbilityLoader.Get(abilityId);

                if (ability != null)
                {
                    race.abilities.Add(ability);
                }
                else
                {
                    Debug.LogError("Ability " + abilityId + " could not be found! Check the folder in Assets/Resources/Scripts/Abilities/Data");
                }
            }

            try
            {
                loaded.Add(race.id, race);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Race " + race.name + " found duplicated! Check the folder in " + dir.ToString());
            }
        }

        SetLoaded();
    }
}
