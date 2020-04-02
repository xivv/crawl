using UnityEngine;

public class InventorySlot : MonoBehaviour
{

    public InventoryItem inventoryItem;
    public ItemSlot itemSlot;


    // Start is called before the first frame update
    void Start()
    {
        inventoryItem = GetComponentInChildren<InventoryItem>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Exchange items with this beeing the drop zone
    public void Exchange(InventorySlot inventorySlot)
    {

        Transform t = inventorySlot.inventoryItem.gameObject.transform.parent;

        if (inventorySlot.inventoryItem != null)
        {
            inventorySlot.inventoryItem.gameObject.transform.parent = gameObject.transform;
            inventorySlot.inventoryItem = null;
        }

        if (inventoryItem != null)
        {
            inventoryItem.gameObject.transform.parent = inventorySlot.gameObject.transform;
            inventoryItem = null;
        }

        // @TODO: Children are empty but it finds items
        foreach (Transform child in transform)
        {
            inventoryItem = child.GetComponent<InventoryItem>();
        }

        // @TODO: Children are empty but it finds items
        foreach (Transform child in inventorySlot.transform)
        {
            inventorySlot.inventoryItem = child.GetComponent<InventoryItem>();
        }
    }

    // Checks if we can exchange items with this beeing the drop zone
    public bool CanExchange(InventorySlot inventorySlot)
    {
        return inventorySlot.itemSlot == itemSlot || itemSlot == ItemSlot.INVENTORY || inventorySlot.inventoryItem.item.itemSlot == itemSlot;
    }

}
