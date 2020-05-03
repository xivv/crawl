using UnityEngine;

public class ActionMenu : MonoBehaviour
{

    public static ActionMenu instance;

    // Panels
    public GameObject panel;

    //Actions
    public GameObject act;
    public GameObject stop;
    public GameObject defend;
    public GameObject flee;
    public GameObject end;

    public GameObject finishSelection;

    // Start is called before the first frame update
    void Start()
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

    }

    public static void NoStandardAction()
    {
        instance.act.SetActive(false);
        instance.stop.SetActive(false);
        instance.defend.SetActive(false);
        instance.flee.SetActive(false);
        instance.end.SetActive(true);
        instance.finishSelection.SetActive(false);
    }

    public static void AbilitySelection()
    {
        instance.act.SetActive(false);
        instance.stop.SetActive(true);
        instance.defend.SetActive(true);
        instance.flee.SetActive(true);
        instance.end.SetActive(true);
        instance.finishSelection.SetActive(true);
    }

    public static void Close()
    {
        instance.panel.SetActive(false);
        instance.act.SetActive(false);
        instance.stop.SetActive(false);
        instance.defend.SetActive(false);
        instance.flee.SetActive(false);
        instance.end.SetActive(false);
        instance.finishSelection.SetActive(false);
    }

    public static void Open()
    {
        instance.panel.SetActive(true);
        instance.act.SetActive(true);
        instance.stop.SetActive(false);
        instance.defend.SetActive(true);
        instance.flee.SetActive(true);
        instance.end.SetActive(true);
        instance.finishSelection.SetActive(false);
    }
}
