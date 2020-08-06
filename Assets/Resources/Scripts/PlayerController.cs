using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public List<Unit> heroes = new List<Unit>();
    public List<Quest> journal = new List<Quest>();

    public static PlayerController instance;

    public static Vector3 position;
    public static Direction direction;

    public static List<Item> GetAllItems()
    {
        List<Item> allItems = new List<Item>();

        foreach (Unit unit in instance.heroes)
        {
            // Check for equipped items aswell? NO?
            foreach (Item item in unit.items)
            {
                allItems.Add(item);
            }
        }

        return allItems;
    }

    public static Quest GetQuest(int id)
    {
        foreach (Quest quest in instance.journal)
        {
            if (id == quest.id)
            {
                return quest;
            }
        }

        return null;
    }


    public static bool QualifiesForQuest(List<int> quests)
    {

        foreach (int id in quests)
        {

            Quest choiceQuest = QuestLoader.Get(id);

            foreach (int questId in choiceQuest.requisites)
            {

                Quest reqQuest = QuestLoader.Get(questId);

                if (!HasQuest(reqQuest.id) || !GetQuest(reqQuest.id).done)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool HasQuest(int id)
    {
        foreach (Quest quest in instance.journal)
        {
            if (id == quest.id)
            {
                return true;
            }
        }

        return false;
    }

    public static void NewQuest(Quest quest)
    {
        instance.journal.Add(quest);
        Debug.Log("New Quest " + quest.description);
        // Aniation etc.
    }

    public static void SetCanMove(bool can)
    {
        WorldMap.player.GetComponent<MovingObject>().pausedMovement = !can;
    }

    public static bool CanMove()
    {
        return !WorldMap.player.GetComponent<MovingObject>().pausedMovement;
    }

    public static List<UnitOrderObject> GenerateHeroes()
    {

        List<UnitOrderObject> list = new List<UnitOrderObject>();

        foreach (Unit unit in instance.heroes)
        {
            GameObject newObject = new GameObject();
            newObject.AddComponent<UnitOrderObject>();
            newObject.AddComponent<BoxCollider2D>();
            newObject.GetComponent<UnitOrderObject>().unit = unit;
            newObject.AddComponent<SpriteRenderer>();
            newObject.GetComponent<SpriteRenderer>().sprite = unit.GetSprite();
            newObject.GetComponent<SpriteRenderer>().sortingLayerName = "Units";
            newObject.layer = 9;
            newObject.name = unit.name;
            list.Add(newObject.GetComponent<UnitOrderObject>());
        }

        return list;
    }

    public static void AwardLoot(List<Item> loot)
    {
        foreach (Item item in loot)
        {
            instance.heroes[0].items.Add(item);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one player instance, dont create a new one");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnGUI()
    {

        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))
        {

            bool isAbilityMenuActive = AbilityMenu.instance.gameObject.activeSelf;

            if (!isAbilityMenuActive)
            {
                MainMenu.instance.IngameMenu();
            }
        }
        else if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Return.ToString())) || Event.current.Equals(Event.KeyboardEvent(KeyCode.KeypadEnter.ToString())))
        {
            if (CanMove())
            {
                // Check if NPC or Interactable is in Range         
                MovementDirection direction = WorldMap.player.GetComponent<MovingObject>().GetDirection();
                Vector3 newPosition = new Vector3();

                if (direction == MovementDirection.EAST)
                {
                    newPosition = position + new Vector3(1, 0);
                }
                else if (direction == MovementDirection.WEST)
                {
                    newPosition = position + new Vector3(-1, 0);
                }
                else if (direction == MovementDirection.NORTH)
                {
                    newPosition = position + new Vector3(0, 1);
                }
                else if (direction == MovementDirection.SOUTH)
                {
                    newPosition = position + new Vector3(0, -1);
                }

                GameObject gameObject = GridTools.GetInteractableAtPosition(newPosition, 9);

                if (gameObject != null)
                {
                    gameObject.GetComponent<Interactable>().Interact();
                }
            }
        }
    }
}
