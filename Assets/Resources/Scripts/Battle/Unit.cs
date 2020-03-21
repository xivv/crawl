using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Unit
{
    /** Encounter Variables **/
    [HideInInspector]
    public string unitName;
    public Sprite sprite;
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

    [HideInInspector]
    public bool isDead = false;
    public bool hasStandardAction = true;

    public Unit(String unitName, String spriteName, MetaInformation metaInformation, Stats baseStats, TypeClass typeClass)
    {
        this.unitName = unitName;
        this.metaInformation = metaInformation;
        this.baseStats = baseStats;
        this.encounterStats = this.baseStats;
        this.typeClass = typeClass;
        this.sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
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
        else
        {
            this.encounterStats.health += change;
        }

        if (this.encounterStats.health <= 0)
        {
            Debug.Log(this.unitName + " died.");
            this.isDead = true;
        }
    }
}

public struct MetaInformation
{
    public int level;
    public int exp;
}

[System.Serializable]
public struct Stats
{
    public int health;
    public int ac;
    public int init;
    public int speed;

    public int bab;

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public int fortitude;
    public int reflex;
    public int will;
}

public enum ConditionType
{
    RAGE,
    FEARED,
    BLEEDING,
    POISONED,
    BLINDED,
    STUNNED,
    ENTANGLED,
    BLESSED,
    REGENERATING,
    HASTE,
    CURSED,
    DEFENSE
}

public enum DamageType
{
    UNTYPED,
    BLEED,
    ACID,
    FIRE,
    COLD,
    ENERGY,
    HOLY,
    UNHOLY,
    MAGIC,
    POISON
}

public enum TargetType
{
    ALLY,
    ENEMY,
    ALL,
    SELF,
    OTHER
}

public enum TypeClass
{
    HERO,
    MONSTER,
    TARGETSELECTOR,
    ALL
}

public enum TargetForm
{
    NORMAL,
    CONE,
    LINE
}

public enum TargetStartPoint
{
    SELF,
    REGION
}

public enum TargetPolygon
{
    RECTANGLE,
    CONE
}

public enum Direction
{
    EAST,
    WEST,
    NORTH,
    SOUTH
}

public enum Vision
{
    DARKVISION,
    LOWLIGHTVISION,
    BLIND
}
