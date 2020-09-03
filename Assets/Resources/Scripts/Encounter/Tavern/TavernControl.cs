using System.Collections.Generic;
using UnityEngine;

public class TavernControl
{

    // ID >> List of Units
    private static Dictionary<int, List<Hero>> taverns = new Dictionary<int, List<Hero>>();

    private static List<Hero> GenerateHeroes()
    {

        List<Hero> newHeroes = new List<Hero>();

        int numberOfHeros = Random.Range(1, 5);

        for (var i = 0; i < numberOfHeros; i++)
        {
            newHeroes.Add(HeroBreeder.Breed(4));
        }
        return newHeroes;
    }

    public static void LoadTavern(int id)
    {
        if (!taverns.ContainsKey(id))
        {
            taverns[id] = GenerateHeroes();
        }

        Tavern.Show(taverns[id], id);
    }

    public static void UpdateTavern(int id, Hero hero)
    {
        taverns[id].Remove(hero);
        PlayerController.instance.heroes.Add(hero);
    }

    public static int RegisterTavern()
    {

        int id = taverns.Count;

        taverns[id] = GenerateHeroes();

        return id;
    }

    public static List<Hero> GetHeroes(int id)
    {
        return taverns[id];
    }
}
