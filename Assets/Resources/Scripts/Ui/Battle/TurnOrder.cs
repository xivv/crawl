using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder : MonoBehaviour
{

    public static TurnOrder instance;

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

    public static void Reload(UnitOrderObject unitToAct, List<UnitOrderObject> units)
    {

        foreach (Transform child in instance.transform)
        {
            Destroy(child.gameObject);
        }

        instance.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 75 * units.Count);

        int indexOfUnitToAct = units.IndexOf(unitToAct);

        for (var i = indexOfUnitToAct; i < units.Count; i++)
        {


            UnitOrderObject unit = units[i];
            GameObject slotPrefab = Resources.Load<GameObject>("Prefabs/Ui/TurnorderSlot");
            GameObject newObject = Instantiate(slotPrefab, instance.transform);
            newObject.GetComponent<TurnorderSlot>().text.text = unit.unit.name + " - " + unit.rolledInit;

            if (i == indexOfUnitToAct)
            {
                newObject.GetComponent<Image>().color = Color.cyan;
            }
        }

        for (var i = 0; i < indexOfUnitToAct; i++)
        {

            UnitOrderObject unit = units[i];
            GameObject slotPrefab = Resources.Load<GameObject>("Prefabs/Ui/TurnorderSlot");
            GameObject newObject = Instantiate(slotPrefab, instance.transform);
            newObject.GetComponent<TurnorderSlot>().text.text = unit.unit.name + " - " + unit.rolledInit;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
