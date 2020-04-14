public class Skeleton : Unit
{
    public Skeleton() : base(
        "Skeleton",
        "skeleton",
       new MetaInformation
       {
           level = 1,
           exp = 0
       },
        new Stats
        {
            health = 4,
            ac = 12,
            init = -2,
            speed = 4,

            bab = 1,

            strength = 1,
            dexterity = 0,
            constitution = 1,
            intelligence = 0,
            wisdom = 0,
            charisma = 0,

            fortitude = 0,
            reflex = 0,
            will = 0
        }
       , TypeClass.MONSTER,
        Size.MEDIUM)
    {
        this.visions.Add(Vision.DARKVISION);
        this.EquipItem(ItemLoader.GetItem("Longsword"));
    }
}
