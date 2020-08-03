using System;
using System.Collections.Generic;

[Serializable]
public class DialogChoice
{
    // id of choice
    public int id;
    public List<int> requisites;
    public string choice;
    public int answer;

}
