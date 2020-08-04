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
        ,
         RaceLoader.Get(1),
         Size.MEDIUM)
    {
        HeroClassLoader.Get(0).ApplyLevelUp(this, 1);
        HeroClassLoader.Get(1).ApplyLevelUp(this, 3);

        visions.Add(Vision.DARKVISION);
        EquipItem(ItemLoader.Get(1));
        items.Add(ItemLoader.Get(0));
    }
}
