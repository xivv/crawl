using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{

    public Ability ability;
    public bool selected = false;

    private Image image;
    private Text text;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = Color.green;
        text = GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ability != null)
        {
            text.text = ability.name;
        }
    }

    public void Hover()
    {
        image.color = Color.yellow;
    }

    public void Unhover()
    {
        image.color = Color.green;
    }

    public void Select()
    {
        if (selected)
        {
            Deselect();
        }
        else
        {
            selected = true;
            image.color = Color.cyan;
        }
    }

    public void Deselect()
    {
        Hover();
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
