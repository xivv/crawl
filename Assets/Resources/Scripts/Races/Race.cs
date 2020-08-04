using System.Collections.Generic;

public class Race : Identifier
{

    public string name;
    public Stats stats;
    public List<Ability> abilities = new List<Ability>();
    public List<Vision> visions = new List<Vision>();
}
