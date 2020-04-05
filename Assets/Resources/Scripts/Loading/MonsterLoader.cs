using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterLoader : Loadable
{

    public static MonsterLoader instance;

    public Dictionary<string, Monster> loadedMonsters = new Dictionary<string, Monster>();
    public DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Scripts/Units/Monster/");

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

    public static Monster GetMonster(string name)
    {
        return instance.loadedMonsters[name];
    }

    public void Load()
    {

        FileInfo[] info = dir.GetFiles("*.json");

        foreach (var file in info)
        {
            string json = File.ReadAllText(file.FullName);
            MonsterWrapper wrapper = JsonUtility.FromJson<MonsterWrapper>(json);
            Monster monster = wrapper;

            monster.typeClass = TypeClass.MONSTER;
            monster.encounterStats = monster.baseStats;

            foreach (string abilityName in wrapper.abilityNames)
            {
                Ability ability = AbilityLoader.GetAbility(abilityName);

                if (ability != null)
                {
                    monster.abilities.Add(ability);
                }
                else
                {
                    Debug.LogError("Ability " + abilityName + " could not be found! Check the folder in Assets/Resources/Scripts/Abilities/Data");
                }
            }

            try
            {
                loadedMonsters.Add(monster.name, monster);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Monster " + monster.name + " found duplicated! Check the folder in " + dir.FullName);
            }
        }

        SetLoaded();
    }
}
