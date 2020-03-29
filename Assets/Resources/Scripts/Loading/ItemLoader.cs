
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{

    public static ItemLoader instance;

    public Dictionary<string, Item> loadedItems = new Dictionary<string, Item>();
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Scripts/Items/Data");

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

    public static Item GetItem(string name)
    {
        return instance.loadedItems[name];
    }

    public void Load()
    {

        FileInfo[] info = dir.GetFiles("*.json");

        foreach (var file in info)
        {
            string json = File.ReadAllText(file.FullName);
            Item item = JsonUtility.FromJson<Item>(json);

            foreach (string abilityName in item.abilityNames)
            {
                Ability ability = AbilityLoader.GetAbility(abilityName);

                if (ability != null)
                {
                    item.abilities.Add(ability);
                }
                else
                {
                    Debug.LogError("Ability " + abilityName + " could not be found! Check the folder in Assets/Resources/Scripts/Abilities/Data");
                }
            }

            try
            {
                loadedItems.Add(item.name, item);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Item " + item.name + " found duplicated! Check the folder in Assets/Resources/Scripts/Items/Data");
            }
        }
    }
}
