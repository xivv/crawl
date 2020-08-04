using System.Collections.Generic;

public class Item : Identifier
{
    public string name;
    public Rarity rarity;
    public string description;
    public Stats stats;
    public List<Ability> abilities = new List<Ability>();

    public ItemSlot itemSlot;
}
