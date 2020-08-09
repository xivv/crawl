using System;
using System.Collections.Generic;

[Serializable]
public class Answer
{
    public int id;
    public List<string> text;
    public bool repeatable = true;
}
