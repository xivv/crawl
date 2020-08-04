using System;
using System.IO;
using UnityEngine;

public class QuestLoader : Loadable<Quest>
{
    public static QuestLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Quest");

    public static Quest Get(int id)
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
            QuestWrapper wrapp = JsonUtility.FromJson<QuestWrapper>(json);
            Quest quest = wrapp;

            foreach (int id in wrapp.requiredItemIds)
            {
                quest.requiredItems.Add(ItemLoader.Get(id));
            }

            foreach (int id in wrapp.lootItemIds)
            {
                quest.loot.Add(ItemLoader.Get(id));
            }

            try
            {
                loaded.Add(quest.id, quest);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Quest " + quest.id + " found duplicated! Check the folder in " + dir.ToString());
            }
        }

        SetLoaded();
    }
}
