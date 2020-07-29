using System.Collections.Generic;


public class HeroClass
{

    public string name;
    public List<ClassProgress> progress = new List<ClassProgress>();


    public void ApplyLevelUp(Hero hero)
    {

        int classLevel = hero.GetClassLevel(this) + 1;

        hero.abilities.AddRange(progress[classLevel].abilities);
        hero.visions.AddRange(progress[classLevel].visions);
        hero.progression[this] = classLevel;

        StatsTools.CalculateStats(progress[classLevel].stats, hero.baseStats, true);

    }

    public void ApplyLevelUp(Hero hero, int level)
    {
        for (int i = 0; i < level; i++)
        {
            ApplyLevelUp(hero);
        }
    }

}
