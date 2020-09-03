using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tavern : MonoBehaviour
{

    public static Tavern instance;

    private static List<Hero> heroesAvailable;
    private static int tavernId;

    public CharacterSlot[] characterSlots;
    public GameObject characterSlotParent;

    private void Awake()
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

    private void Start()
    {
        Render();
    }

    public static void Hide()
    {
        SceneManager.LoadScene("WorldMap");
    }

    public static void Show(List<Hero> heroes, int id)
    {
        tavernId = id;
        SceneManager.LoadScene("Tavern");
        heroesAvailable = heroes;
    }

    public void Clear()
    {
        foreach (Transform child in characterSlotParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Render()
    {
        Clear();

        foreach (Hero hero in heroesAvailable)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Encounter/Tavern/CharacterSlot");
            GameObject newObject = Instantiate(prefab, characterSlotParent.transform);
            newObject.GetComponent<CharacterSlot>().hero = hero;
        }

        characterSlots = GetComponentsInChildren<CharacterSlot>();
    }

    public static void Select(Hero hero)
    {
        TavernControl.UpdateTavern(tavernId, hero);
        heroesAvailable.Remove(hero);
        instance.Render();
    }
}
