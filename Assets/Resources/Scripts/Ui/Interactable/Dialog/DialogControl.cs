﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogControl : ControllingElement
{
    public static DialogControl instance;
    private static InteractableDialog interactableDialog;
    private static Queue<string> queue = new Queue<string>();

    public GameObject panel;
    public Text textElement;
    private bool choosing = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (instance != null && interactableDialog != null)
        {
            instance.isAwake = true;
            textElement.text = queue.Peek();
        }
    }

    public static void Choose(DialogChoice dialogChoice)
    {
        instance.ClearChoices();
        interactableDialog.Choose(dialogChoice);
        instance.choosing = false;
        ShowAnswer(interactableDialog.GetAnswer(dialogChoice.answer));
        instance.panel.SetActive(false);
    }

    public static void StopDialog()
    {

        instance.isAwake = false;

        // Check for repeatable dialogChoices

        foreach (DialogChoice dialogChoice in interactableDialog.choices)
        {
            if (dialogChoice.repeatable)
            {

                // Check for exits this will happen with quests where you can say no but want to come back later to accept the quest
                if (dialogChoice.exits.Count == 0)
                {
                    interactableDialog.used.Remove(dialogChoice.id);
                }
                else if (dialogChoice.exits.Count > 0)
                {
                    foreach (int id in dialogChoice.exits)
                    {
                        if (!interactableDialog.used.Contains(id))
                        {
                            interactableDialog.used.Remove(dialogChoice.id);
                        }
                        else if (interactableDialog.used.Contains(id) && interactableDialog.choosen.Contains(dialogChoice.id))
                        {
                            interactableDialog.used.Remove(id);
                            interactableDialog.used.Remove(dialogChoice.id);
                        }
                    }
                }
            }
        }

        // Reset dialog but we maybe wanna decide the state on different values
        interactableDialog.dialogState = DialogState.OPENING;
        interactableDialog.choosen.Clear();
        instance.textElement.text = "";
        SceneManager.UnloadSceneAsync("DialogModule");
        PlayerController.SetCanMove(true);
        PartyQuickInfo.Show();

    }


    public static void StartDialog(int id)
    {
        StartDialog(DialogLoader.Get(id));
    }

    private static bool ShowAnswer(Answer answer)
    {
        if (answer.repeatable || (!answer.repeatable && !interactableDialog.answered.Contains(answer.id)))
        {
            queue = new Queue<string>(answer.text);
            interactableDialog.answered.Add(answer.id);
            return true;
        }

        Progress();
        return false;
    }

    public static void StartDialog(InteractableDialog intercomDialog)
    {
        SceneManager.LoadSceneAsync("DialogModule", LoadSceneMode.Additive);
        PlayerController.SetCanMove(false);
        PartyQuickInfo.Hide();
        interactableDialog = intercomDialog;
        ShowAnswer(interactableDialog.GetOpener());
    }

    void ClearChoices()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private static void CreateExitButton()
    {
        GameObject exitButton = Resources.Load<GameObject>("Prefabs/Ui/Dialog/DialogChoiceExit");
        Instantiate(exitButton, instance.panel.transform);
    }

    private static void CreateChoiceButton(DialogChoice dialogChoice)
    {
        GameObject newChoiceButton = Resources.Load<GameObject>("Prefabs/Ui/Dialog/DialogChoiceSlot");
        newChoiceButton.GetComponent<DialogChoiceSlot>().dialogChoice = dialogChoice;
        Instantiate(newChoiceButton, instance.panel.transform);
    }


    public static void Progress()
    {

        if (!instance.choosing)
        {
            if (queue.Count > 1 && !instance.choosing)
            {
                queue.Dequeue();
            }
            else if (interactableDialog.dialogState == DialogState.OPENING || interactableDialog.dialogState == DialogState.CHOICE)
            {

                // We check if we have available answers
                List<DialogChoice> choices = interactableDialog.GetAvailableChoices();

                if (choices.Count > 0)
                {
                    // Enable Choice Selection
                    instance.choosing = true;

                    interactableDialog.dialogState = DialogState.CHOICE;

                    foreach (DialogChoice choice in choices)
                    {
                        CreateChoiceButton(choice);
                    }
                    CreateExitButton();
                    instance.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 700);
                    instance.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300 * choices.Count);
                    instance.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 1000);
                    instance.panel.SetActive(true);
                }
                else
                {

                    interactableDialog.dialogState = DialogState.ENDING;

                    if (interactableDialog.GetEnding() != null)
                    {
                        ShowAnswer(interactableDialog.GetEnding());
                    }
                    else
                    {
                        Progress();
                    }
                }
            }
            else if (interactableDialog.dialogState == DialogState.ENDING)
            {
                StopDialog();
            }
        }
    }

    public new static bool IsKeyBlocking()
    {
        if (instance == null)
        {
            return false;
        }

        return instance.isAwake;
    }
}
