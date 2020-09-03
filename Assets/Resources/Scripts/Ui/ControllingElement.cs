using UnityEngine;

public class ControllingElement : MonoBehaviour
{

    // Keycontroll enabled
    protected bool canAct;

    // Is in top focus disabling other elements
    protected bool isAwake;

    // @KeyController decides which events are called by pressing buttons
    public static bool IsKeyBlocking()
    {
        return false;
    }


}
