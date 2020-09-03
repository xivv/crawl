using System.Collections.Generic;
using UnityEngine;

public class BlockEvent : GameEvent
{

    public int sucessDialogId;
    public int failureDialogId;

    public List<int> requiredItemIds;
    public List<GameObject> toBeDestroyed;

    public override void SubCall()
    {
        if (PlayerController.HasItems(requiredItemIds))
        {
            if (sucessDialogId > 0)
            {
                DialogControl.StartDialog(sucessDialogId);

            }

            foreach (GameObject gameObject in toBeDestroyed)
            {
                Destroy(gameObject);
            }

            if (repeatable)
            {
                triggered = false;
            }
        }
        else
        {
            if (failureDialogId > 0)
            {
                DialogControl.StartDialog(failureDialogId);
                WorldMap.MoveBack();
            }
            triggered = false;
        }
    }
}
