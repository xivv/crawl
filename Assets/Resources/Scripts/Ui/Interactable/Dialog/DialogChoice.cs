using System;
using System.Collections.Generic;

[Serializable]
public class DialogChoice
{
    // id of choice
    public int id;
    public List<int> requisites = new List<int>();
    public string choice;
    public int answer;

    public List<int> exits = new List<int>();
    public List<int> quest = new List<int>();
}
