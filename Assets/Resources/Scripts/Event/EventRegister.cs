using System.Collections.Generic;
using UnityEngine;

public class EventRegister : ScriptableObject
{
    private static Dictionary<int, bool> events = new Dictionary<int, bool>();

    public static void Register(GameEvent gameEvent)
    {
        events[gameEvent.id] = gameEvent.triggered;
    }

    public static bool GetTriggerState(int id)
    {

        if (events.ContainsKey(id))
        {
            return events[id];
        }
        else
        {
            return false;
        }
    }
}
