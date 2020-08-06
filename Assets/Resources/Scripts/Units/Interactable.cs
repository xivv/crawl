using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int dialogId;

    public virtual void Interact()
    {
        CheckQuestFinished();
    }

    // Ids of quests that can be finished here
    // We will check this on interact

    public int[] questFinisher;


    public void CheckQuestFinished()
    {
        foreach (int id in questFinisher)
        {

            Quest quest = PlayerController.GetQuest(id);

            if (quest != null && quest.IsSuccessFull(PlayerController.GetAllItems()))
            {
                List<Item> loot = quest.OnSuccess();
            }
        }
    }
}
