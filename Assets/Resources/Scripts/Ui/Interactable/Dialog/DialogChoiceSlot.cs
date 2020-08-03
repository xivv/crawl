using UnityEngine;
using UnityEngine.UI;

public class DialogChoiceSlot : MonoBehaviour
{

    public DialogChoice dialogChoice;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = dialogChoice.choice;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeChoice()
    {
        DialogControl.Choose(dialogChoice);
    }
}
