using System.Collections.Generic;
using UnityEngine;

public class Loadable : MonoBehaviour
{

    private bool isLoaded = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsLoaded()
    {
        return isLoaded;
    }

    public void SetLoaded()
    {
        isLoaded = true;
    }

    public virtual void Load()
    {

    }

    public void LoadAbilities(List<string> abilityNames, List<Ability> target)
    {
        foreach (string abilityName in abilityNames)
        {
            Ability ability = AbilityLoader.GetAbility(abilityName);

            if (ability != null)
            {
                target.Add(ability);
            }
            else
            {
                throw new System.Exception("Ability " + abilityName + " could not be found! Check the folder in Assets/Resources/Scripts/Abilities/Data");
            }
        }
    }
}
