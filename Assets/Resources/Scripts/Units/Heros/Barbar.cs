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
       }, new List<HeroClass>(new HeroClass[] {

           }),
       RaceLoader.GetRace("Demon"),
         Size.MEDIUM)
    {
        this.visions.Add(Vision.DARKVISION);
        this.EquipItem(ItemLoader.GetItem("Longsword"));
    }
}
