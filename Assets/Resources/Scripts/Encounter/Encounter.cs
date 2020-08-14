using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    public int? encounterId;
    public EncounterType encounterType;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        Sprite sprite = Resources.Load<Sprite>("Sprites/Encounter/" + encounterType);

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Sprite for EncounterType " + encounterType + " not available");
        }
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
                encounterId = TavernControl.LoadTavern(encounterId);
                break;
            case EncounterType.SHOP:
                SceneManager.LoadScene("ItemShop");
                break;
        }
    }
}
