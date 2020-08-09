using System;
using System.Collections.Generic;

[Serializable]
public class InteractableDialog
{
    public List<int> used;
    public List<int> choosen;
    public List<int> answered;

    public int id;
    public int opening;
    public int ending;
    public List<Answer> answers;
    public List<DialogChoice> choices;

    public DialogState dialogState = DialogState.OPENING;

    public DialogChoice GetChoice(int id)
    {

        foreach (DialogChoice choice in choices)
        {
            if (choice.id == id)
            {
                return choice;

            }
        }

        return null;
    }

    public Answer GetAnswer(int id)
    {

        foreach (Answer answer in answers)
        {
            if (answer.id == id)
            {
                return answer;

            }
        }

        return null;
    }

    public void Choose(DialogChoice dialogChoice)
    {
        used.Add(dialogChoice.id);
        used.AddRange(dialogChoice.exits);
        choosen.Add(dialogChoice.id);

        foreach (int id in dialogChoice.quest)
        {
            PlayerController.NewQuest(QuestLoader.Get(id));
        }

        foreach (int id in dialogChoice.items)
        {
            PlayerController.AwardLoot(ItemLoader.Get(id));
        }
    }

    public Answer GetOpener()
    {

        foreach (Answer answer in answers)
        {
            if (answer.id == opening)
            {
                return answer;
            }
        }

        return null;
    }

    public Answer GetEnding()
    {

        foreach (Answer answer in answers)
        {
            if (answer.id == ending)
            {
                return answer;
            }
        }

        return null;
    }

    public List<DialogChoice> GetAvailableChoices()
    {
        List<DialogChoice> possible = new List<DialogChoice>(choices);


        foreach (DialogChoice choice in choices)
        {
            foreach (int requisites in choice.requisites)
            {
                if (!used.Contains(requisites))
                {
                    possible.Remove(choice);
                    break;
                }
            }
        }

        List<DialogChoice> available = new List<DialogChoice>(possible);

        foreach (DialogChoice choice in possible)
        {
            if (used.Contains(choice.id) || !PlayerController.QualifiesForQuest(choice.quest))
            {
                available.Remove(choice);
            }
        }


        return available;
    }
}

public enum DialogState
{
    OPENING,
    CHOICE,
    ENDING
}
