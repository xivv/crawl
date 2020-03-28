using System.Collections.Generic;

public class Item
{

    public Stats stats;
    public List<Ability> abilities = new List<Ability>();

    public ItemSlot itemSlot;

    public Item(Stats stats, ItemSlot itemSlot)
    {
        this.stats = stats;
        this.itemSlot = itemSlot;
    }
}
