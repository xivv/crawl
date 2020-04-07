using System.Collections.Generic;

public abstract class Hero : Unit
{
    public List<HeroClass> heroClasses = new List<HeroClass>();
    public Race race;

    public Hero(string unitName, string spriteName, MetaInformation metaInformation, Stats baseStats, List<HeroClass> heroClasses, Race race) : base(unitName, spriteName, metaInformation, baseStats, TypeClass.HERO)
    {
        this.heroClasses = heroClasses;
        this.race = race;

        // We apply the race modifier
        CalculateStats(race.stats, true);

        // We add all race abilities to the unit
        foreach (var ability in race.abilities)
        {
            abilities.Add(ability);
        }
    }

    public int getLevelsOfClass(HeroClass targetHeroClass)
    {
        int counter = 0;

        foreach (HeroClass heroClass in this.heroClasses)
        {
            if (heroClass == targetHeroClass) counter++;
        }

        return counter;
    }
}

public enum HeroClass
{
    CLERIC,
    BARBAR,
    WIZARD
}
