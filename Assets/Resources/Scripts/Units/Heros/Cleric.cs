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

            }),
         RaceLoader.GetRace("Human"),
         Size.MEDIUM)
    {
        visions.Add(Vision.DARKVISION);
        EquipItem(ItemLoader.GetItem("Longsword"));
        items.Add(ItemLoader.GetItem("HeavyShield"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
        abilities.Add(AbilityLoader.GetAbility("Bless"));
    }
}
