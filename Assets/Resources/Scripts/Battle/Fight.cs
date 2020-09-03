using System.Collections.Generic;

public class Fight : Identifier
{

    // DialogIds
    public int opening;
    public int onWin;
    public int onLoose;

    public List<Unit> enemies = new List<Unit>();
    public List<Unit> allies = new List<Unit>();
    public List<Item> loot = new List<Item>();


    public List<Unit> GetAllUnits()
    {
        List<Unit> allUnits = new List<Unit>();

        foreach (Unit unit in enemies)
        {
            allUnits.Add(unit);
        }

        foreach (Unit unit in allies)
        {
            allUnits.Add(unit);
        }

        return allUnits;
    }
}
