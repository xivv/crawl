using System.Collections.Generic;

public class Item
{
    public string name;
    public Rarity rarity;
    public string description;
    public Stats stats;
    public List<Ability> abilities = new List<Ability>();

    // We use this to load items from json with string names of abilities
    // And add them in the itemloader afterwards
    public List<string> abilityNames = new List<string>();

    public ItemSlot itemSlot;
}
