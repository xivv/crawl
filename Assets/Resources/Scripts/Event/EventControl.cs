using System.Collections.Generic;
using UnityEngine;

public class EventControl : MonoBehaviour
{

    public static EventControl instance;

    private Queue<GameEvent> events = new Queue<GameEvent>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (events.Count > 0)
        {
            events.Dequeue().Call();
        }
    }

    public static void Register(GameEvent gameEvent)
    {
        instance.events.Enqueue(gameEvent);
    }
}
