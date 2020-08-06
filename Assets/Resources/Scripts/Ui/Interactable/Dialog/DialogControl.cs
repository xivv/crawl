using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
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

        //DialogControl.StartDialog(DialogLoader.GetDialog(0));
    }

    // Update is called once per frame
    void Update()
    {
        if (instance != null && interactableDialog != null)
        {
            textElement.text = queue.Peek();
        }
    }

    public static void Choose(DialogChoice dialogChoice)
    {
        instance.ClearChoices();
        interactableDialog.Choose(dialogChoice);
        instance.choosing = false;
        queue = new Queue<string>(interactableDialog.GetAnswer(dialogChoice.answer).text);
        instance.panel.SetActive(false);
    }

    public static void StopDialog()
    {

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

    public static void StartDialog(InteractableDialog intercomDialog)
    {
        SceneManager.LoadSceneAsync("DialogModule", LoadSceneMode.Additive);
        PlayerController.SetCanMove(false);
        PartyQuickInfo.Hide();
        interactableDialog = intercomDialog;
        queue = new Queue<string>(interactableDialog.GetOpener().text);
    }

    void ClearChoices()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void CreateExitButton()
    {
        GameObject exitButton = Resources.Load<GameObject>("Prefabs/Ui/Dialog/DialogChoiceExit");
        Instantiate(exitButton, panel.transform);
    }

    void CreateChoiceButton(DialogChoice dialogChoice)
    {
        GameObject newChoiceButton = Resources.Load<GameObject>("Prefabs/Ui/Dialog/DialogChoiceSlot");
        newChoiceButton.GetComponent<DialogChoiceSlot>().dialogChoice = dialogChoice;
        Instantiate(newChoiceButton, panel.transform);
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.KeypadEnter.ToString())) || Event.current.Equals(Event.KeyboardEvent(KeyCode.Return.ToString())))
        {

            if (!choosing)
            {
                if (queue.Count > 1 && !choosing)
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
                        choosing = true;
                        instance.panel.SetActive(true);
                        instance.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 300 * choices.Count);

                        interactableDialog.dialogState = DialogState.CHOICE;



                        foreach (DialogChoice choice in choices)
                        {
                            CreateChoiceButton(choice);
                        }
                        CreateExitButton();
                    }
                    else
                    {

                        if (interactableDialog.GetEnding() != null)
                        {
                            queue = new Queue<string>(interactableDialog.GetEnding().text);
                        }
                        interactableDialog.dialogState = DialogState.ENDING;
                    }
                }
                else if (interactableDialog.dialogState == DialogState.ENDING)
                {
                    StopDialog();
                }
            }
        }
    }
}
