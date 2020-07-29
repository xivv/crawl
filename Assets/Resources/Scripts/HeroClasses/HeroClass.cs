using System.Collections.Generic;


public class HeroClass
{

    public string name;
    public List<ClassProgress> progress = new List<ClassProgress>();

    void ApplyLevelUp(Hero hero, int level)
    {
        hero.abilities.AddRange(progress[level].abilities);
        hero.visions.AddRange(progress[level].visions);
        StatsTools.CalculateStats(progress[level].stats, hero.baseStats, true);
    }

}
