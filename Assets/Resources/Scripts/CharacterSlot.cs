using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{

    public Image image;
    public Text text;

    Hero hero;

    public void Awake()
    {
        this.image = GetComponent<Image>();
    }

    public void InsertHero(Hero hero)
    {
        this.hero = hero;
        this.image.sprite = Resources.Load<Sprite>("Sprites/" + hero.name);
        this.text.text = hero.heroClasses[0].ToString();
    }

    public void ClearHero()
    {
        this.hero = null;
        this.image.sprite = null;
        this.text.text = "";
    }

    public void OnSelectHeroButton()
    {
        PlayerController.instance.heroes.Add(this.hero);
        ClearHero();
        SceneManager.LoadScene("WorldMap");
    }
}
