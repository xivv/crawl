using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public int dialogId;
    public abstract void Interact();

    // Ids of quests that can be finished here
    // We will check this on interact

    public int[] questFinisher;


    public void CheckQuestFinished()
    {
        foreach (int id in questFinisher)
        {

            Quest quest = PlayerController.GetQuest(id);

            if (quest != null && quest.IsSuccessFull(null))
            {
                quest.OnSuccess();
            }
        }
    }
}
