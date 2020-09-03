public class FightEvent : GameEvent
{
    public int fightId;
    public int battleFieldId;


    public override void SubCall()
    {
        FightControl.StartFight(fightId, battleFieldId);
    }

}
