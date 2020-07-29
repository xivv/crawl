using System.Collections.Generic;

public abstract class Hero : Unit
{
    public List<HeroClass> heroClasses = new List<HeroClass>();
    public Race race;
    public bool levelUpAvailable = false;

    public Hero(string unitName, string spriteName, MetaInformation metaInformation, Stats baseStats, List<HeroClass> heroClasses, Race race, Size size) : base(unitName, spriteName, metaInformation, baseStats, TypeClass.HERO, size)
    {
        this.heroClasses = heroClasses;
        this.race = race;

        // We apply the race modifier
        StatsTools.CalculateStats(race.stats, baseStats, true);

        // We add all race abilities to the unit
        foreach (var ability in race.abilities)
        {
            abilities.Add(ability);
        }
    }


    public void LevelUp()
    {

    }

    public void AwardExperience(int experience)
    {
        metaInformation.exp += experience;

        int experienceForNextlevel = HeroAdvancement.table[this.metaInformation.level + 1];

        if (metaInformation.exp >= experienceForNextlevel)
        {
            levelUpAvailable = true;
        }

    }

    public int GetClassLevel(HeroClass targetHeroClass)
    {
        int counter = 0;

        foreach (HeroClass heroClass in this.heroClasses)
        {
            if (heroClass == targetHeroClass) counter++;
        }

        return counter;
    }
}
