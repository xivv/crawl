using System.Collections.Generic;
using UnityEngine;

public class AbilityEffect
{
    // Conditions
    public List<Condition> conditions = new List<Condition>();
    public TargetType targetType;
    public int? dc;
    public string savingThrow;

    // Damage
    public int? damageDie;
    public int? damageDice;
    public DamageType damageType;
    public bool confirmHit;

    // Bonus on either DC of ability or to hit
    public string abilityScoreBonus;

    public AbilityEffect(List<Condition> conditions, TargetType targetType)
    {
        this.conditions = conditions;
        this.targetType = targetType;
    }

    public AbilityEffect(List<Condition> conditions, TargetType targetType, int? dc, string savingThrow, bool confirmHit, string abilityScoreBonus)
    {
        this.conditions = conditions;
        this.targetType = targetType;
        this.dc = dc;
        this.savingThrow = savingThrow;
        this.confirmHit = confirmHit;
        this.abilityScoreBonus = abilityScoreBonus;
    }

    public AbilityEffect(TargetType targetType, int damageDie, int damageDice, DamageType damageType, bool confirmHit, string abilityScoreBonus)
    {
        this.targetType = targetType;
        this.damageDie = damageDie;
        this.damageDice = damageDice;
        this.damageType = damageType;
        this.confirmHit = confirmHit;
        this.abilityScoreBonus = abilityScoreBonus;
    }

    public AbilityEffect(List<Condition> conditions, TargetType targetType, int? dc, string savingThrow, int damageDie, int damageDice, DamageType damageType, bool confirmHit, string abilityScoreBonus)
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

    public bool hitSucceded(Unit source, Unit target)
    {
        return this.rollDie(1, 20) + (int)typeof(Stats).GetField(this.abilityScoreBonus).GetValue(source.encounterStats) >=
            target.encounterStats.ac + target.encounterStats.dexterity;
    }

    public bool SavingThrowSucceded(Unit source, Unit target)
    {
        if (this.dc == null) return false;
        return this.rollDie(1, 20) + (int)typeof(Stats).GetField(this.savingThrow).GetValue(target.encounterStats) >=
            this.dc + (int)typeof(Stats).GetField(this.abilityScoreBonus).GetValue(source.encounterStats);
    }
}
