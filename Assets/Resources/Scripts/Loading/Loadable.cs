using System.Collections.Generic;
using UnityEngine;

public class Loadable<T> : LoadableEscape
{

    protected Dictionary<int, T> loaded = new Dictionary<int, T>();

    protected T GetRandom()
    {
        return loaded[Random.Range(0, loaded.Keys.Count)];
    }

    public void LoadAbilities(List<int> abilityIds, List<Ability> target)
    {
        foreach (int abilityId in abilityIds)
        {
            Ability ability = AbilityLoader.Get(abilityId);

            if (ability != null)
            {
                target.Add(ability);
            }
            else
            {
                throw new System.Exception("Ability " + abilityId + " could not be found! Check the folder in Assets/Resources/Scripts/Abilities/Data");
            }
        }
    }
}
