using System.Collections.Generic;

public class Barbar : Hero
{
    public Barbar(string unitName) : base(
        "Barbar",
        "player",
       new MetaInformation
       {
           level = 1,
           exp = 0
       },
       new Stats
       {
           health = 12,
           ac = 14,
           init = 1,
           speed = 6,

           bab = 1,

           strength = 2,
           dexterity = 0,
           constitution = 2,
           intelligence = 0,
           wisdom = 0,
           charisma = 0,

           fortitude = 1,
           reflex = 0,
           will = 0
       }
       , new List<HeroClass>(new HeroClass[] {
                HeroClass.BARBAR
           }))
    {



        Ability smash = new Ability("Smash", "Description", 1, 1, 1, new List<AbilityEffect>(
           new AbilityEffect[] {
            new AbilityEffect(TargetType.ALL, 2, -6, DamageType.UNTYPED, true, "strength")
           }
       ));

        Ability rage = new Ability("Barbarians Rage", "Description", 1, 1, 0, new List<AbilityEffect>(
            new AbilityEffect[] {
                new AbilityEffect(new List<Condition>(new Condition[] {
                new Condition(ConditionType.RAGE,4,this)
            }), TargetType.SELF)
            }
        ));

        this.abilities.Add(rage);
        this.abilities.Add(smash);
    }
}
