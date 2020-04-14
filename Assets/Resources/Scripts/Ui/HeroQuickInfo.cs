using UnityEngine;
using UnityEngine.UI;

public class HeroQuickInfo : MonoBehaviour
{

    public Unit unit;

    public Image image;
    public Text text;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        image.sprite = unit.GetSprite();
        text.text = unit.name;
        slider.maxValue = unit.baseStats.health;
        slider.value = unit.encounterStats.health;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
