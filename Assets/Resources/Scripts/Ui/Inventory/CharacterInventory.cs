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
            this.character = new Cleric("Herbert");
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
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Close()
    {
        SceneManager.UnloadSceneAsync("CharacterInventory");
    }

    void LoadItems()
    {
        foreach (KeyValuePair<ItemSlot, Item> entry in character.equippedItems)
        {
            if (equippedItems.ContainsKey(entry.Key))
            {
                InventorySlot slot = equippedItems[entry.Key];
                GameObject item = Resources.Load<GameObject>("Prefabs/InventoryItem");
                Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
                item.transform.parent = slot.gameObject.transform;
            }
        }

        foreach (var entry in character.items)
        {
            foreach (var possibleSlot in inventoryItems)
            {
                if (possibleSlot.inventoryItem == null)
                {

                }
            }
        }
    }
}
