using System;
using System.IO;
using UnityEngine;

public class BattleFieldLoader : Loadable<BattleField>
{

    public static BattleFieldLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/BattleField");


    public static BattleField Get(int id)
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
            BattleField battleField = JsonUtility.FromJson<BattleField>(json);

            try
            {
                loaded.Add(battleField.id, battleField);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Battlefield " + battleField.id + " found duplicated! Check the folder in " + dir.ToString());
            }
        }

        SetLoaded();
    }
}
