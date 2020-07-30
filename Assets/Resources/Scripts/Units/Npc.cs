using UnityEngine;

public class Npc : Interactable
{
    public override void Interact()
    {
        Talk();
    }

    public void Talk()
    {
        Debug.Log("Hello");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
