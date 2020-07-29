using System;
using System.Collections.Generic;

[Serializable]
public class ClassProgress
{
    public List<Ability> abilities = new List<Ability>();
    public List<string> abilityNames = new List<string>();
    public List<Vision> visions = new List<Vision>();
    public Stats stats;

}
