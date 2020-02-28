using UnityEngine;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour
{

    public UnitOrderObject unitOrderObject;
    public Text unitName;
    public Text unitAc;
    public Text remainingMovement;
    public GameObject unitHealth;
    public Text unitHealthText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (unitOrderObject != null && unitOrderObject.unit != null)
        {
            unitName.text = unitOrderObject.unit.unitName;
            unitAc.text = unitOrderObject.unit.encounterStats.ac + " AC";
            remainingMovement.text = unitOrderObject.remainingMovementSpeed + " stepps left";
            unitHealth.GetComponent<Slider>().maxValue = unitOrderObject.unit.baseStats.health;
            unitHealth.GetComponent<Slider>().value = unitOrderObject.unit.encounterStats.health;
            unitHealthText.text = unitOrderObject.unit.encounterStats.health + "/" + unitOrderObject.unit.baseStats.health;
        }

    }
}
