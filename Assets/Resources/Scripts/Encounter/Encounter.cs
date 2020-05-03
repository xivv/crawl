using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{

    public EncounterType encounterType;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnter()
    {
        switch (encounterType)
        {
            case EncounterType.BATTLE:
                SceneManager.LoadScene("BattleMap");
                break;
            case EncounterType.TAVERN:
                SceneManager.LoadScene("Tavern");
                break;
            case EncounterType.SHOP:
                SceneManager.LoadScene("ItemShop");
                break;
        }
    }
}
