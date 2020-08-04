
using System;
using System.IO;
using UnityEngine;

public class ItemLoader : Loadable<Item>
{

    public static ItemLoader instance;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/Items/");

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Item Get(int id)
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
            ItemWrapper itemWrapper = JsonUtility.FromJson<ItemWrapper>(json);
            Item item = itemWrapper;

            LoadAbilities(itemWrapper.abilityIds, item.abilities);

            try
            {
                loaded.Add(item.id, item);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Item " + item.name + " found duplicated! Check the folder in Assets/Resources/Scripts/Items/Data");
            }
        }

        SetLoaded();
    }
}
