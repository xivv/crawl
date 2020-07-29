using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    /** Encounter Variables **/
    [HideInInspector]
    public string name;

    [HideInInspector]
    public MetaInformation metaInformation = new MetaInformation();

    public Stats baseStats;
    public Stats encounterStats;
    public TypeClass typeClass;

    public List<Vision> visions = new List<Vision>();
    public List<Ability> abilities = new List<Ability>();
    public List<DamageType> resistances = new List<DamageType>();
    public List<DamageType> weaknesses = new List<DamageType>();
    public List<Condition> conditions = new List<Condition>();

    public List<Item> items = new List<Item>();
    public Dictionary<ItemSlot, Item> equippedItems = new Dictionary<ItemSlot, Item>();

    public Size size;


    [HideInInspector]
    public bool isDead = false;
    public bool hasStandardAction = true;

    public Unit(String unitName, String spriteName, MetaInformation metaInformation, Stats baseStats, TypeClass typeClass, Size size)
    {
        this.name = unitName;
        this.metaInformation = metaInformation;
        this.baseStats = baseStats;
        this.encounterStats = this.baseStats;
        this.typeClass = typeClass;
        this.size = size;
    }

    public void UnequipItem(Item item)
    {
        Item alreadyEquiped = equippedItems[item.itemSlot];

        if (alreadyEquiped != null)
        {
            items.Add(alreadyEquiped);
            equippedItems.Remove(item.itemSlot);
            StatsTools.CalculateStats(item.stats, baseStats, false);

            foreach (var ability in item.abilities)
            {
                this.abilities.Remove(ability);
            }
        }
    }

    public void EquipItem(Item item)
    {

        try
        {
            // We do this since we could also use this method to force equip an item
            // This removes the item from the inventory
            if (items.Contains(item))
            {
                items.Remove(item);
            }

            equippedItems.Add(item.itemSlot, item);
        }
        catch (ArgumentException)
        {
            // Already one item equipped, swap items
            UnequipItem(equippedItems[item.itemSlot]);
            equippedItems.Add(item.itemSlot, item);
        }

        StatsTools.CalculateStats(item.stats, baseStats, true);

        if (item.abilities.Count > 0)
        {
            this.abilities.AddRange(item.abilities);
        }
    }

    public bool IsStunned()
    {
        foreach (Condition condition in this.conditions)
        {
            if (condition.conditionType == ConditionType.STUNNED)
            {
                return true;
            }
        }
        return false;
    }

    public void handleHealthChange(int change, DamageType damageType)
    {
        if (this.resistances.Contains(damageType))
        {
            change = (int)Math.Floor((decimal)(change / 2));
        }
        else if (this.weaknesses.Contains(damageType))
        {
            change = change * 2;
        }

        // TODO: People can overheal from this
        if (damageType == DamageType.HEALING)
        {
            this.encounterStats.health += change;
        }
        else
        {
            this.encounterStats.health -= change;
        }

        if (this.encounterStats.health <= 0)
        {
            Debug.Log(this.name + " died.");
            this.isDead = true;
        }
    }

    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/" + name);
    }

    public void AddCondition(int duration, ConditionType conditionType, Unit source)
    {
        conditions.Add(new Condition(conditionType, duration, source));
    }
}
