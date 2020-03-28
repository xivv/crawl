using System.Collections.Generic;

public class Cleric : Hero
{
    public Cleric(string unitName) : base("Cleric", "player",
        new MetaInformation
        {
            level = 1,
            exp = 0
        },
        new Stats
        {
            health = 10,
            ac = 16,
            init = 0,
            speed = 4,

            bab = 1,

            strength = 1,
            dexterity = 0,
            constitution = 1,
            intelligence = 0,
            wisdom = 2,
            charisma = 0,

            fortitude = 0,
            reflex = 0,
            will = 1
        }
        , new List<HeroClass>(new HeroClass[] {
                HeroClass.CLERIC
            }))
    {
        AbilityEffect blessEffect = new AbilityEffect(new List<Condition>(new Condition[] {
                new Condition(ConditionType.BLESSED,2)
            }), TargetType.ALLY);

        Ability bless = new Ability("Bless", "Description", null, null, 6, new List<AbilityEffect>(
            new AbilityEffect[] {
                blessEffect
            }
        ));
        Ability clw = new Ability("Cure Light Wounds", "Description", null, null, 6, new List<AbilityEffect>(
           new AbilityEffect[] {
                blessEffect
           }
       ));

        this.abilities.Add(clw);
        this.abilities.Add(bless);
        this.visions.Add(Vision.DARKVISION);
    }
}
