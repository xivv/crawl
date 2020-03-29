﻿using System.Collections.Generic;

public class Wizard : Hero
{

    public Wizard(string unitName) : base("Wizard", "player",
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
          intelligence = 100,
          wisdom = 0,
          charisma = 0,

          fortitude = 1,
          reflex = 0,
          will = 0
      }
      , new List<HeroClass>(new HeroClass[] {
                HeroClass.WIZARD
          }))
    {

        AbilityEffect firebreathEffect = new AbilityEffect(new List<Condition>(new Condition[] {
                new Condition(ConditionType.CURSED,2)
            }), TargetType.ALL, 10, SavingThrow.REFLEX, false, AbilityScore.CHARISMA);

        Ability fireBreath = new Ability("Firebreath", "Description", 1, 0, 1, TargetStartPoint.REGION, TargetPolygon.CONE, 2, new List<AbilityEffect>(
            new AbilityEffect[] {
                firebreathEffect
            }
        ));

        AbilityEffect abilityEffect = new AbilityEffect(TargetType.ALL, 10, 6, DamageType.FIRE, false, AbilityScore.INTELLIGENCE);
        Ability ability = new Ability("Fireball", "Description", 1, 0, 3, TargetStartPoint.REGION, 2, new List<AbilityEffect>(
            new AbilityEffect[] {
                abilityEffect
            }
        ));

        this.abilities.Add(ability);
        this.abilities.Add(fireBreath);
    }
}
