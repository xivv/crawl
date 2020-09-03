using UnityEngine;

public class GameEvent : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    public int id;

    public bool render;
    public bool repeatable;
    public bool triggered;
    public bool executed;

    // Start is called before the first frame update
    void Start()
    {
        triggered = EventRegister.GetTriggerState(id);

        if (!render)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }

    public void Call()
    {
        if (!triggered || repeatable)
        {
            triggered = true;
            SubCall();
            EventRegister.Register(this);
        }
    }

    public virtual void AfterCall()
    {

    }

    public virtual void SubCall()
    {

    }
}
