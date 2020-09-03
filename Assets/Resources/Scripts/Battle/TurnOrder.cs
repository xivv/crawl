using System.Collections.Generic;
using System.Linq;

public class TurnOrder2
{
    SortedDictionary<double, UnitOrderObject> order = new SortedDictionary<double, UnitOrderObject>();
    double index = double.MinValue;

    public void Initialise(List<UnitOrderObject> unitOrderObjects)
    {
        foreach (UnitOrderObject unitOrderObject in unitOrderObjects)
        {

            double rolledInitiative = unitOrderObject.RollInitiative();

            while (order.ContainsKey(rolledInitiative))
            {
                rolledInitiative = unitOrderObject.RollInitiative();
            }

            order.Add(rolledInitiative, unitOrderObject);
        }

    }

    public UnitOrderObject GetActiveUnitOrderObject()
    {
        return order[index];
    }

    public UnitOrderObject Next()
    {

        foreach (double initiative in order.Keys)
        {
            if (index < initiative)
            {
                index = initiative;
                return order[index];
            }
        }

        // End of turn
        index = order.Keys.Min();
        return order[index];
    }

}
