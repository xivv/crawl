public class Npc : Interactable
{

    public override void Interact()
    {
        base.Interact();
        Talk();
    }

    public void Talk()
    {
        DialogControl.StartDialog(dialogId);

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
