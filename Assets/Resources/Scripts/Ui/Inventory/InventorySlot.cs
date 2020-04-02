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

    public void AddNewChild(Item newItem)
    {
        GameObject newGameObject = new GameObject();
        newGameObject = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
        newGameObject.AddComponent(typeof(InventoryItem));

        newGameObject.transform.SetParent(gameObject.transform);

        if (newItem != null)
        {
            newGameObject.GetComponent<InventoryItem>().item = newItem;
        }

    }
    public void AddNewChild()
    {
        AddNewChild(null);
    }

    public void ReloadChild()
    {
        // @TODO: Children are empty but it finds items
        foreach (Transform child in transform)
        {
            inventoryItem = child.GetComponent<InventoryItem>();
        }
    }

    // Exchange items with this beeing the drop zone
    public void Exchange(InventorySlot inventorySlot)
    {

        if (inventorySlot.inventoryItem != null)
        {
            inventorySlot.inventoryItem.gameObject.transform.SetParent(gameObject.transform);
            inventorySlot.inventoryItem = null;
        }

        if (inventoryItem != null)
        {
            inventoryItem.gameObject.transform.SetParent(inventorySlot.gameObject.transform);
            inventoryItem = null;
        }

        ReloadChild();
        inventorySlot.ReloadChild();
    }

    // Checks if we can exchange items with this beeing the drop zone
    public bool CanExchange(InventorySlot inventorySlot)
    {
        return inventorySlot.itemSlot == itemSlot || itemSlot == ItemSlot.INVENTORY || inventorySlot.inventoryItem.item.itemSlot == itemSlot;
    }

}
