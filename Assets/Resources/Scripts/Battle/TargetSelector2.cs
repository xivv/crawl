using System.Collections.Generic;
using UnityEngine;

public class TargetSelector2 : MonoBehaviour
{

    public static TargetSelector2 instance;

    public bool canAct = false;
    public Ability ability;
    public Vector3 startPoint;

    public List<UnitOrderObject> selectedUnitOrderObjects = new List<UnitOrderObject>();

    public static void StartSelection(Ability ability, Vector3 startPoint)
    {
        instance.ability = ability;
        instance.startPoint = startPoint;
        instance.canAct = true;
    }

    public static void StopSelection()
    {
        instance.canAct = false;
    }

    private void Select()
    {
        if (ability.IsAOE())
        {
            //this.SelectAOETargets(ability);
        }
        // If we can only target ourself
        else if (ability.IsSelfTargeting())
        {
            //  instance.selectedUnitOrderObjects.Add(source);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one target selector instance found");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (canAct)
        {
            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.KeypadEnter.ToString())) || Event.current.Equals(Event.KeyboardEvent(KeyCode.Return.ToString())))
            {

                // Validate Target

            }
        }
    }
}
