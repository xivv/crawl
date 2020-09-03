public class DialogEvent : GameEvent
{

    public int dialogId;

    public override void SubCall()
    {
        DialogControl.StartDialog(dialogId);
    }

}
