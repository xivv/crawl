using UnityEngine;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour
{

    public UnitOrderObject unitOrderObject;

    public Text unitName;
    public GameObject health;
    public GameObject energy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (unitOrderObject != null && unitOrderObject.unit != null)
        {
            unitName.text = unitOrderObject.unit.name;

            health.GetComponent<Slider>().maxValue = unitOrderObject.unit.baseStats.health;
            health.GetComponent<Slider>().value = unitOrderObject.unit.encounterStats.health;

            energy.GetComponent<Slider>().maxValue = unitOrderObject.unit.encounterStats.speed;
            energy.GetComponent<Slider>().value = unitOrderObject.remainingMovementSpeed;
        }

    }
}
