using System;
using System.Collections.Generic;

[Serializable]
public class Item
{
    public string name;
    public string description;
    public Stats stats;
    public List<Ability> abilities;

    // We use this to load items from json with string names of abilities
    // And add them in the itemloader afterwards
    public List<string> abilityNames;

    public ItemSlot itemSlot;

    public Item(string name, string description, Stats stats, ItemSlot itemSlot)
    {
        this.name = name;
        this.description = description;
        this.stats = stats;
        this.itemSlot = itemSlot;
    }
}
