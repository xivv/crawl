using System;
using System.Collections.Generic;

[Serializable]
public class MonsterWrapper : Monster
{
    public List<int> abilityIds = new List<int>();
    public List<int> itemIds = new List<int>();
    public List<int> equippedItemIds = new List<int>();

    public MonsterWrapper(string unitName, string spriteName, MetaInformation metaInformation, Stats baseStats, Size size) : base(unitName, spriteName, metaInformation, baseStats, size)
    {
    }
}
