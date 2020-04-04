
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AbilityLoader : Loadable
{

    public static AbilityLoader instance;

    public Dictionary<string, Ability> loadedAbilities = new Dictionary<string, Ability>();
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Scripts/Abilities/Data");

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

    public static Ability GetAbility(string name)
    {
        return instance.loadedAbilities[name];
    }

    public void Load()
    {

        FileInfo[] info = dir.GetFiles("*.json");

        foreach (var file in info)
        {
            string json = File.ReadAllText(file.FullName);
            AbilityWrapper abilityWrapper = JsonUtility.FromJson<AbilityWrapper>(json); ;
            Ability ability = abilityWrapper;

            try
            {
                loadedAbilities.Add(ability.name, ability);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Ability " + ability.name + " found duplicated! Check the folder in Assets/Resources/Scripts/Abilities/Data");
            }
        }

        SetLoaded();
    }
}
