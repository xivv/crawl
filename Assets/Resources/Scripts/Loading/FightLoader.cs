using System;
using System.IO;
using UnityEngine;

public class FightLoader : Loadable<Fight>
{
    public static FightLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Fight");


    public static Fight Get(int id)
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
            FightWrapper fightWrapper = JsonUtility.FromJson<FightWrapper>(json);
            Fight fight = fightWrapper;


            foreach (int id in fightWrapper.enemyIds)
            {
                fight.enemies.Add(MonsterLoader.Get(id));
            }

            foreach (int id in fightWrapper.allyIds)
            {
                fight.allies.Add(MonsterLoader.Get(id));
            }

            foreach (int id in fightWrapper.lootIds)
            {
                fight.loot.Add(ItemLoader.Get(id));
            }

            try
            {
                loaded.Add(fight.id, fight);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Fight " + fight.id + " found duplicated! Check the folder in " + dir.ToString());
            }
        }

        SetLoaded();
    }

}
