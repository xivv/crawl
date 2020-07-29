using System;
using System.Collections.Generic;

[Serializable]
public class MonsterWrapper : Monster
{
    public List<string> abilityNames = new List<string>();
    public List<string> itemNames = new List<string>();
    public List<string> equippedItemNames = new List<string>();

    public MonsterWrapper(string unitName, string spriteName, MetaInformation metaInformation, Stats baseStats, Size size) : base(unitName, spriteName, metaInformation, baseStats, size)
    {
    }
}
