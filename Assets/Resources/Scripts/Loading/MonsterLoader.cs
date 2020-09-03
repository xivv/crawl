using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterLoader : Loadable<Monster>
{

    public static MonsterLoader instance;
    public DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Monster/");

    public static Monster Get(int id)
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
            MonsterWrapper wrapper = JsonUtility.FromJson<MonsterWrapper>(json);
            Monster monster = wrapper;

            monster.typeClass = TypeClass.MONSTER;
            monster.encounterStats = monster.baseStats;

            foreach (int abilityId in wrapper.abilityIds)
            {
                Ability ability = AbilityLoader.Get(abilityId);

                if (ability != null)
                {

                    if (monster.abilities == null)
                    {
                        monster.abilities = new List<Ability>();
                    }

                    monster.abilities.Add(ability);
                }
                else
                {
                    Debug.LogError("Ability " + abilityId + " could not be found! Check the folder in Assets/Resources/Scripts/Abilities/Data");
                }
            }

            try
            {
                loaded.Add(monster.id, monster);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Monster " + monster.name + " found duplicated! Check the folder in " + dir.FullName);
            }
        }

        SetLoaded();
    }
}
