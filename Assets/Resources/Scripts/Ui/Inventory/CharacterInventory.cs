using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterInventory : MonoBehaviour
{

    public static CharacterInventory instance;

    public Unit character;

    private Dictionary<ItemSlot, InventorySlot> equippedItems = new Dictionary<ItemSlot, InventorySlot>();
    private List<InventorySlot> inventoryItems = new List<InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            this.character = PlayerController.instance.heroes[0];
        }
        else
        {
            Destroy(this);
        }

        // EquippedItems
        foreach (var inventorySlot in GetComponentsInChildren<InventorySlot>())
        {

            if (inventorySlot.itemSlot == ItemSlot.INVENTORY)
            {
                inventoryItems.Add(inventorySlot);

            }
            else
            {
                equippedItems.Add(inventorySlot.itemSlot, inventorySlot);
            }
        }

        LoadItems();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Close()
    {

        // Update our character
        character.items.Clear();
        character.equippedItems.Clear();

        foreach (KeyValuePair<ItemSlot, InventorySlot> entry in equippedItems)
        {
            if (entry.Value.inventoryItem != null)
            {
                character.equippedItems.Add(entry.Key, entry.Value.inventoryItem.item);
            }
        }

        // We shouldnt organize what players did this could become a feature
        // TODO: option to make organizing not automatic
        foreach (var entry in inventoryItems)
        {
            if (entry.inventoryItem != null)
            {
                character.items.Add(entry.inventoryItem.item);
            }
        }

        SceneManager.UnloadSceneAsync("CharacterInventory");

        if (Encounter.instance != null && !Encounter.instance.isRunning)
        {
            Encounter.Resume();
        }
    }

    void LoadItems()
    {
        foreach (KeyValuePair<ItemSlot, Item> entry in character.equippedItems)
        {
            if (equippedItems.ContainsKey(entry.Key))
            {
                InventorySlot slot = equippedItems[entry.Key];
                slot.AddNewChild(entry.Value);
            }
        }

        foreach (var entry in character.items)
        {
            foreach (var possibleSlot in inventoryItems)
            {
                if (possibleSlot.inventoryItem == null)
                {
                    possibleSlot.AddNewChild(entry);
                    break;
                }
            }
        }
    }
}
