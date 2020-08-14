using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tavern : MonoBehaviour
{
    private static List<Hero> heroesAvailable;

    public CharacterSlot[] characterSlots;
    public GameObject characterSlotParent;

    private void Start()
    {
        foreach (Hero hero in heroesAvailable)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Encounter/Tavern/CharacterSlot");
            GameObject newObject = Instantiate(prefab, characterSlotParent.transform);
            newObject.GetComponent<CharacterSlot>().hero = hero;
        }

        characterSlots = GetComponentsInChildren<CharacterSlot>();
    }

    public static void Show(List<Hero> heroes)
    {
        SceneManager.LoadScene("Tavern");
        heroesAvailable = heroes;
    }
}
