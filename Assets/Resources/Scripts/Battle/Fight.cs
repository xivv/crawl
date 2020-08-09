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

}
