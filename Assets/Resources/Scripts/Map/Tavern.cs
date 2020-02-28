using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tavern : MonoBehaviour
{
    public int maxHeroesAvailable = 4;
    public CharacterSlot[] characterSlots;

    void Awake()
    {
        characterSlots = GetComponentsInChildren<CharacterSlot>();
    }

    // Start is called before the first frame update
    void Start()
    {
        generateNewHeroes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void generateNewHeroes()
    {
        for (var i = 0; i < maxHeroesAvailable; i++)
        {
            int random = Random.Range(0, Enum.GetNames(typeof(HeroClass)).Length);
            characterSlots[i].InsertHero(generateRandomHero((HeroClass)random));
        }
    }

    public Hero generateRandomHero(HeroClass heroClass)
    {

        if (heroClass == HeroClass.BARBAR)
        {
            return new Barbar("Herbert");
        }
        else if (heroClass == HeroClass.CLERIC)
        {
            return new Cleric("Ghoran");
        }
        else if (heroClass == HeroClass.WIZARD)
        {
            return new Wizard("Merlin");
        }
        else
        {
            return null;
        }
    }
}
