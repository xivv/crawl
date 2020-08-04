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
        instance.textElement.text = "";
        SceneManager.UnloadSceneAsync("DialogModule");
        PlayerController.SetCanMove(true);
        PartyQuickInfo.Show();

    }

    public static void StartDialog(InteractableDialog intercomDialog)
    {
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
                        instance.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 150 * choices.Count);

                        interactableDialog.dialogState = DialogState.CHOICE;



                        foreach (DialogChoice choice in choices)
                        {
                            CreateChoiceButton(choice);
                        }
                    }
                    else
                    {
                        queue = new Queue<string>(interactableDialog.GetEnding().text);
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
