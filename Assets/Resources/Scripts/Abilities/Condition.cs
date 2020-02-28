public class Condition
{
    public ConditionType conditionType;
    public int remainingTime;
    public int duration;
    public Unit source;

    public Condition(ConditionType conditionType, int duration, Unit source)
    {
        this.conditionType = conditionType;
        this.remainingTime = duration;
        this.duration = duration;
        this.source = source;
    }

}
