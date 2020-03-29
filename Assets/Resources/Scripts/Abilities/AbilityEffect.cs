﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AbilityEffect
{
    // Conditions
    public List<Condition> conditions = new List<Condition>();
    public TargetType targetType;
    public int? dc;
    public SavingThrow savingThrow;

    // Damage
    public int? damageDie;
    public int? damageDice;
    public DamageType damageType;
    public bool confirmHit;

    // Bonus on either DC of ability or to hit
    public AbilityScore abilityScoreBonus;

    public AbilityEffect(List<Condition> conditions, TargetType targetType)
    {
        this.conditions = conditions;
        this.targetType = targetType;
    }

    public AbilityEffect(List<Condition> conditions, TargetType targetType, int? dc, SavingThrow savingThrow, bool confirmHit, AbilityScore abilityScoreBonus)
    {
        this.conditions = conditions;
        this.targetType = targetType;
        this.dc = dc;
        this.savingThrow = savingThrow;
        this.confirmHit = confirmHit;
        this.abilityScoreBonus = abilityScoreBonus;
    }

    public AbilityEffect(TargetType targetType, int damageDie, int damageDice, DamageType damageType, bool confirmHit, AbilityScore abilityScoreBonus)
    {
        this.targetType = targetType;
        this.damageDie = damageDie;
        this.damageDice = damageDice;
        this.damageType = damageType;
        this.confirmHit = confirmHit;
        this.abilityScoreBonus = abilityScoreBonus;
    }

    public AbilityEffect(List<Condition> conditions, TargetType targetType, int? dc, SavingThrow savingThrow, int damageDie, int damageDice, DamageType damageType, bool confirmHit, AbilityScore abilityScoreBonus)
    {
        this.conditions = conditions;
        this.targetType = targetType;
        this.dc = dc;
        this.savingThrow = savingThrow;
        this.damageDie = damageDie;
        this.damageDice = damageDice;
        this.damageType = damageType;
        this.confirmHit = confirmHit;
        this.abilityScoreBonus = abilityScoreBonus;
    }

    public int rollDie(int? dice, int? die)
    {
        if (die < 0)
        {
            dice *= -1;
        }
        return (int)Random.Range((float)dice, (float)die);
    }

    public int rollDamage()
    {
        return rollDie(damageDice, damageDie);
    }

    public int GetAbilityScoreBonus(Unit unit, AbilityScore abilityScore)
    {
        switch (abilityScoreBonus)
        {
            case AbilityScore.STRENGTH:
                return unit.encounterStats.strength;
            case AbilityScore.DEXTERITY:
                return unit.encounterStats.dexterity;
            case AbilityScore.CONSTITUTION:
                return unit.encounterStats.constitution;
            case AbilityScore.INTELLIGENCE:
                return unit.encounterStats.intelligence;
            case AbilityScore.WISDOM:
                return unit.encounterStats.wisdom;
            case AbilityScore.CHARISMA:
                return unit.encounterStats.charisma;
            default:
                return 0;
        }
    }

    public int GetSavingThrowBonus(Unit unit, SavingThrow savingThrow)
    {
        switch (savingThrow)
        {
            case SavingThrow.FORITUDE:
                return unit.encounterStats.fortitude;
            case SavingThrow.REFLEX:
                return unit.encounterStats.reflex;
            case SavingThrow.WILL:
                return unit.encounterStats.will;
            default:
                return 0;
        }
    }

    public bool hitSucceded(Unit source, Unit target)
    {

        int rolled = rollDie(1, 20);
        int hitBonus = GetAbilityScoreBonus(source, abilityScoreBonus);

        return rolled + hitBonus + hitBonus >=
            target.encounterStats.ac + target.encounterStats.dexterity;
    }

    public bool SavingThrowSucceded(Unit source, Unit target)
    {
        // No saving throw allowed
        if (this.dc == null)
        {
            return false;
        }

        int rolled = rollDie(1, 20);
        int saveBonus = GetSavingThrowBonus(target, savingThrow);
        int dcBonus = GetAbilityScoreBonus(source, abilityScoreBonus);

        return rolled + saveBonus >= this.dc + dcBonus;
    }
}
