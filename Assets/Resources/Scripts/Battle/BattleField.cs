using System.Collections.Generic;

public class BattleField : Identifier
{
    public int width;
    public int height;

    // we use this to make specific fields a specific id
    public Dictionary<int, int> fieldIds;
    public Terrain terrain;
    public TimeStatus timeStatus;
}
