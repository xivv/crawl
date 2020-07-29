using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{

    public Image image;
    public Text clazz;
    public Text race;

    Hero hero;

    public void Awake()
    {
        this.image = GetComponent<Image>();
    }

    public void InsertHero(Hero hero)
    {
        this.hero = hero;
        image.sprite = Resources.Load<Sprite>("Sprites/" + hero.name);
        clazz.text = "";
        race.text = hero.race.name;

        foreach (HeroClass heroClass in hero.progression.Keys)
        {
            clazz.text += heroClass.name + " " + hero.progression[heroClass] + "\n";
        }

    }

    public void ClearHero()
    {
        hero = null;
        image.sprite = null;
        clazz.text = "";
        race.text = "";
    }

    public void OnSelectHeroButton()
    {
        PlayerController.instance.heroes.Add(this.hero);
        ClearHero();
        SceneManager.LoadScene("WorldMap");
    }
}
