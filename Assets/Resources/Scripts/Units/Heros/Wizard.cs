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
      ,
       RaceLoader.Get(1),
         Size.MEDIUM)
    {

    }
}
