using System.Collections.Generic;
using UnityEngine;

public class EncounterRegister : ScriptableObject
{
    private static List<int> ids = new List<int>();

    public static int GetNextId()
    {
        if (ids.Count > 0)
        {
            return ids[ids.Count - 1] + 1;
        }
        else
        {
            return 0;
        }
    }

    public static void Register(GameObject gameObject, int id)
    {

        if (!ids.Contains(id))
        {
            ids.Add(id);

        }
        else
        {
            //    Debug.LogError("GameObject: " + gameObject.name + " id " + id + " already registered. Next id is " + GetNextId());
        }
    }
}
