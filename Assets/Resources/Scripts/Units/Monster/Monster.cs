public class Monster : Unit
{

    public Monster(string unitName, string spriteName, MetaInformation metaInformation, Stats baseStats, Size size) : base(unitName, spriteName, metaInformation, baseStats, TypeClass.MONSTER, size)
    {
        this.size = size;
    }

}
