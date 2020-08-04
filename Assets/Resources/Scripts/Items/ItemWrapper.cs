using System;
using System.Collections.Generic;

[Serializable]
public class ItemWrapper : Item
{
    // We use this to load items from json with string names of abilities
    // And add them in the itemloader afterwards
    public List<int> abilityIds = new List<int>();
}
