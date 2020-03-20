
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string name;
    public string description;
    public int? minTargets; // If null we do AOE
    public int? maxTargets; // If null we do AOE
    public int reach;

    public TargetStartPoint targetStartPoint;
    public TargetPolygon targetPolygon;

    public int targetArea;

    public List<AbilityEffect> effects = new List<AbilityEffect>();

    public Ability(string name, string description, int? minTargets, int? maxTargets, int reach, List<AbilityEffect> effects)
    {
        this.name = name;
        this.description = description;
        this.minTargets = minTargets;
        this.maxTargets = maxTargets;
        this.reach = reach;
        this.effects = effects;
        this.targetStartPoint = TargetStartPoint.SELF;
    }

    public Ability(string name, string description, int? minTargets, int? maxTargets, int reach, TargetStartPoint targetStartPoint, int targetArea, List<AbilityEffect> effects)
    {
        this.name = name;
        this.description = description;
        this.minTargets = minTargets;
        this.maxTargets = maxTargets;
        this.reach = reach;
        this.effects = effects;
        this.targetStartPoint = targetStartPoint;
        this.targetArea = targetArea;
        this.targetPolygon = TargetPolygon.RECTANGLE;
    }

    public Ability(string name, string description, int? minTargets, int? maxTargets, int reach, TargetStartPoint targetStartPoint, TargetPolygon targetPolygon, int targetArea, List<AbilityEffect> effects)
    {
        this.name = name;
        this.description = description;
        this.minTargets = minTargets;
        this.maxTargets = maxTargets;
        this.reach = reach;
        this.effects = effects;
        this.targetStartPoint = targetStartPoint;
        this.targetArea = targetArea;
        this.targetPolygon = targetPolygon;
    }

    private void ApplyCondition(AbilityEffect effect, Unit target)
    {
        if (effect.conditions.Count > 0)
        {
            foreach (Condition condition in effect.conditions)
            {

                bool alreadyHasCondition = false;

                // We check first if the character already has the condition and if yes we try to make the duration longer
                foreach (Condition targetCondition in target.conditions)
                {
                    if (targetCondition.conditionType == condition.conditionType)
                    {
                        targetCondition.duration += condition.duration;
                        targetCondition.remainingTime += condition.duration;
                        Debug.Log(target.unitName + ":> already has condition " + condition.ToString());
                        alreadyHasCondition = true;
                    }
                }

                if (!alreadyHasCondition)
                {
                    target.conditions.Add(condition);
                    Debug.Log(target.unitName + ":> new condition " + condition.ToString());
                }
            }
        }
    }

    public void DrawReach(UnitOrderObject source, Vector2 startingPosition)
    {
        Vector2 startPosition = new Vector2(source.transform.position.x - this.reach, source.transform.position.y + this.reach);

        if (this.targetPolygon == TargetPolygon.RECTANGLE)
        {
            MapTools.DrawReach(startPosition, startingPosition, this.reach);
        }
    }

    public bool IsSelfTargeting()
    {
        return this.reach == 0;
    }

    public bool IsAOE()
    {
        return this.minTargets == null && this.maxTargets == null;
    }

    public void executeAbility(Unit source, List<Unit> targets)
    {
        foreach (Unit target in targets)
        {
            foreach (AbilityEffect effect in effects)
            {
                // If we cannot target this unit    
                if (effect.targetType == TargetType.SELF && source != target)
                {
                    Debug.Log(target.unitName + ":> cant be targeted by ability because target is not SELF");
                    break;
                }
                else if (effect.targetType == TargetType.OTHER && source == target)
                {
                    Debug.Log(target.unitName + ":> cant be targeted by ability because target is not OTHER");
                    break;
                }
                else if (effect.targetType == TargetType.ALLY && source.typeClass != target.typeClass)
                {
                    Debug.Log(target.unitName + ":> cant be targeted by ability because target is not an ALLY");
                    break;
                }
                else if (effect.targetType == TargetType.ENEMY && source.typeClass == target.typeClass)
                {
                    Debug.Log(target.unitName + ":> cant be targeted by ability because target is not an ENEMY");
                    break;
                }

                // If the ability needs to hit first
                if (effect.confirmHit)
                {
                    if (!effect.hitSucceded(source, target))
                    {
                        Debug.Log(target.unitName + ":> did not get hit by ability");
                        break;
                    }
                    else
                    {
                        target.handleHealthChange(effect.rollDamage(), effect.damageType);
                        Debug.Log(target.unitName + ":> did get hit by the ability");
                    }
                }
                else if (effect.damageDice != null && effect.damageDie != null)
                {
                    target.handleHealthChange(effect.rollDamage(), effect.damageType);
                    Debug.Log(target.unitName + ":> takes damage automatically");
                }

                // We first check if we need to do a saving throw
                if (effect.dc != null)
                {
                    if (effect.SavingThrowSucceded(source, target))
                    {
                        Debug.Log(target.unitName + ":> saving throw succeded");
                        break;
                    }
                    else
                    {
                        Debug.Log(target.unitName + ":> saving throw failed");

                        ApplyCondition(effect, target);
                    }
                }
                else if (effect.conditions != null && effect.conditions.Count > 0 && effect.dc == null)
                {
                    ApplyCondition(effect, target);
                }
            }
        }
    }
}
