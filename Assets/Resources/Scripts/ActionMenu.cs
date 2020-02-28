using UnityEngine;

public class ActionMenu : MonoBehaviour
{
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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void abilitySelection()
    {
        this.act.SetActive(false);
        this.stop.SetActive(true);
        this.defend.SetActive(true);
        this.flee.SetActive(true);
        this.end.SetActive(true);
        this.finishSelection.SetActive(true);
    }

    public void close()
    {
        this.panel.SetActive(false);
        this.act.SetActive(false);
        this.stop.SetActive(false);
        this.defend.SetActive(false);
        this.flee.SetActive(false);
        this.end.SetActive(false);
        this.finishSelection.SetActive(false);
    }

    public void open()
    {
        this.panel.SetActive(true);
        this.act.SetActive(true);
        this.stop.SetActive(false);
        this.defend.SetActive(true);
        this.flee.SetActive(true);
        this.end.SetActive(true);
        this.finishSelection.SetActive(false);
    }
}
