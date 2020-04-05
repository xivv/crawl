using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string name;
    public string description;
    public int minTargets;
    public int maxTargets;
    public int reach;

    public TargetStartPoint targetStartPoint;
    public TargetPolygon targetPolygon;

    public int targetArea;

    public List<AbilityEffect> effects = new List<AbilityEffect>();

    private void ApplyConditions(List<Condition> conditions, Unit source, Unit target)
    {
        foreach (Condition condition in conditions)
        {

            bool alreadyHasCondition = false;

            // We check first if the character already has the condition and if yes we do nothing
            foreach (Condition targetCondition in target.conditions)
            {
                if (targetCondition.conditionType == condition.conditionType)
                {
                    Debug.Log(target.name + ":> already has condition " + condition.ToString());
                    alreadyHasCondition = true;
                    return;
                }
            }

            if (!alreadyHasCondition)
            {
                target.conditions.Add(new Condition(condition.conditionType, condition.duration, source));
                Debug.Log(target.name + ":> new condition " + condition.ToString());
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
        return this.minTargets < 0 && this.maxTargets < 0;
    }

    public void Execute(Unit source, List<Unit> targets)
    {
        foreach (Unit target in targets)
        {
            foreach (AbilityEffect effect in effects)
            {

                // This means we need to hit first, if we dont hit we exit this effect
                if (effect.confirmHit)
                {

                    bool isHit = effect.hitSucceded(source, target);
                    if (!isHit)
                    {
                        Debug.Log(target.name + ":> did not get hit by the ability");
                        break;
                    }
                    else
                    {
                        Debug.Log(target.name + ":> got hit by the ability");
                    }
                }

                // If the ability makes damage
                if (effect.damageDie > 0 && effect.damageDice > 0)
                {
                    target.handleHealthChange(effect.rollDamage(), effect.damageType);
                    Debug.Log(target.name + ":> takes damage");
                }

                // This means that the conditions dont apply automatically and need a saving throw
                // We make a simple version of saving here, we just apply the conditions on a failed save
                if (effect.conditions.Count > 0)
                {
                    if (effect.dc > 0)
                    {
                        bool didSave = effect.SavingThrowSucceded(source, target);

                        if (!didSave)
                        {
                            ApplyConditions(effect.conditions, source, target);
                        }
                    }
                    else
                    {
                        ApplyConditions(effect.conditions, source, target);
                    }
                }
            }
        }
    }
}
