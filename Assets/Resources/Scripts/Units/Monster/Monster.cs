public class Monster : Unit
{

    public Size size;

    public Monster(string unitName, string spriteName, MetaInformation metaInformation, Stats baseStats, Size size) : base(unitName, spriteName, metaInformation, baseStats, TypeClass.MONSTER)
    {
        this.size = size;
    }

}
