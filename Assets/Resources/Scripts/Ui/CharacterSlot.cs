using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{

    public Hero hero;
    public int tavernId;

    private Image image;
    public Text clazz;
    public Text race;
    public Text size;

    public void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Update()
    {
        if (hero != null)
        {
            image.sprite = Resources.Load<Sprite>("Sprites/" + hero.name);
            clazz.text = "";
            race.text = hero.race.name;
            size.text = hero.size.ToString();

            foreach (HeroClass heroClass in hero.progression.Keys)
            {
                clazz.text += heroClass.name + " " + hero.progression[heroClass] + "\n";
            }
        }
    }

    public void OnSelectHeroButton()
    {
        Tavern.Select(hero);
    }
}
