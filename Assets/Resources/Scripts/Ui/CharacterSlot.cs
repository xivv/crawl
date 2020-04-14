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
        this.image.sprite = Resources.Load<Sprite>("Sprites/" + hero.name);
        this.clazz.text = hero.heroClasses[0].ToString();
        this.race.text = hero.race.name;
    }

    public void ClearHero()
    {
        this.hero = null;
        this.image.sprite = null;
        this.clazz.text = "";
        this.race.text = "";
    }

    public void OnSelectHeroButton()
    {
        PlayerController.instance.heroes.Add(this.hero);
        ClearHero();
        SceneManager.LoadScene("WorldMap");
    }
}
