using System.Reflection;

public class StatsTools
{

    public static void CalculateStats(Stats source, Stats target, bool apply)
    {
        foreach (var field in typeof(Stats).GetFields(BindingFlags.Instance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
        {
            int newValue = 0;

            if (apply)
            {
                newValue = (int)field.GetValue(target) + (int)field.GetValue(source);

            }
            else
            {
                newValue = (int)field.GetValue(target) - (int)field.GetValue(source);
            }

            typeof(Stats).GetField(field.Name).SetValue(target, newValue);
        }
    }
}
