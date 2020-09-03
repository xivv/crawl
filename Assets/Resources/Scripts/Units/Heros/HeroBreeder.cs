using UnityEngine;

public class HeroBreeder : ScriptableObject
{

    private static string[] heroNames = new string[]
    {
        "Yasmine Matthams",
        "Amy O'Doherty",
        "Mason Pearce",
        "Ciara Brooks",
        "Alexia Hernandez",
        "Louis Robbins",
        "Marco Stein"
    };


    public static Hero Breed(int level)

    {
        Hero hero = new Hero(
            heroNames[Random.Range(0, heroNames.Length)], "player",
        new MetaInformation
        {
            level = 1,
            exp = 0
        },
        new Stats
        {
            health = 10,
            ac = 16,
            init = 0,
            speed = 4,

            bab = 1,

            strength = 1,
            dexterity = 0,
            constitution = 1,
            intelligence = 0,
            wisdom = 2,
            charisma = 0,

            fortitude = 0,
            reflex = 0,
            will = 1
        }
        ,
         RaceLoader.GenerateRandom()
         );

        foreach (HeroClass heroClass in HeroClassLoader.GenerateHeroClasses(level))
        {
            if (!hero.progression.ContainsKey(heroClass))
            {
                hero.progression[heroClass] = 0;
            }

            hero.progression[heroClass]++;
        }

        return hero;
    }
}
