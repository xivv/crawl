public class Condition
{
    public ConditionType conditionType;

    public int duration;

    // Meta Information
    public Unit source;
    public int remainingTime;

    public Condition(ConditionType conditionType, int duration)
    {
        this.conditionType = conditionType;
        this.remainingTime = duration;
        this.duration = duration;
    }

    public Condition(ConditionType conditionType, int duration, Unit source)
    {
        this.conditionType = conditionType;
        this.remainingTime = duration;
        this.duration = duration;
        this.source = source;
    }

}
