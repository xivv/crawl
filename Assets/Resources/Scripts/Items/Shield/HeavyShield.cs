using System.Collections.Generic;

public class HeavyShield : Item
{

    public HeavyShield() : base(
        new Stats
        {
            health = 0,
            ac = 2,
            init = 0,
            speed = 0,

            bab = 0,

            strength = 0,
            dexterity = 0,
            constitution = 0,
            intelligence = 0,
            wisdom = 0,
            charisma = 0,

            fortitude = 0,
            reflex = 0,
            will = 0
        },
        ItemSlot.SHIELD)
    {
        Ability rage = new Ability("Barbarians Rage", "Description", 1, 1, 0, new List<AbilityEffect>(
        new AbilityEffect[] {
                new AbilityEffect(new List<Condition>(new Condition[] {
                new Condition(ConditionType.RAGE,4,null)
            }), TargetType.SELF)
        }
    ));
        this.abilities.Add(rage);
    }
}
