using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{

    public Hero hero;
    private Image image;
    public Text clazz;
    public Text race;

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

            foreach (HeroClass heroClass in hero.progression.Keys)
            {
                clazz.text += heroClass.name + " " + hero.progression[heroClass] + "\n";
            }
        }
    }

    public void OnSelectHeroButton()
    {
        PlayerController.instance.heroes.Add(this.hero);
        SceneManager.LoadScene("WorldMap");
    }
}
