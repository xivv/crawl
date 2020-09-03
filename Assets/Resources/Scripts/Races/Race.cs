using System.Collections.Generic;
using UnityEngine;

public class Race : Identifier
{

    public string name;
    public Stats stats;
    public List<Ability> abilities = new List<Ability>();
    public List<Vision> visions = new List<Vision>();
    public List<Size> sizes = new List<Size>();

    public Size GetRandomSize()
    {
        return sizes[Random.Range(0, sizes.Count)];
    }
}
