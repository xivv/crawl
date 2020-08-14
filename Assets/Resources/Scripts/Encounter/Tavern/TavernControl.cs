using System.Collections.Generic;

public class TavernControl
{

    // ID >> List of Units
    private static Dictionary<int, List<Hero>> taverns = new Dictionary<int, List<Hero>>();

    private static List<Hero> GenerateHeroes()
    {
        return new List<Hero>(new Hero[] {
            new Cleric("Base"),
            new Cleric("Hammel"),
            new Cleric("Anime")
        });
    }

    public static int LoadTavern(int? id)
    {
        if (id == null)
        {
            int newId = taverns.Count;
            taverns[newId] = GenerateHeroes();
            Tavern.Show(taverns[newId]);
            return newId;
        }
        else
        {
            Tavern.Show(taverns[id.GetValueOrDefault()]);
            return id.GetValueOrDefault();
        }
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
