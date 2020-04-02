using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : DragAndDrop
{

    public Item item;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    protected override void OnDrag()
    {
        base.OnDrag();
        gameObject.transform.SetParent(GetComponentInParent<Canvas>().transform);
    }

    protected override void OnDrop()
    {
        base.OnDrop();

        GameObject possible = GetObjectUnderMouse();

        // We either need to get the InventorySlot from the object or the parent

        if (possible != null)
        {
            InventorySlot slot = possible.GetComponent<InventorySlot>();
            InventorySlot parentSlot = possible.GetComponentInParent<InventorySlot>();

            if (slot != null || parentSlot != null)
            {

                GameObject oldParent = GetOldParent();
                InventorySlot oldParentSlot = oldParent.GetComponent<InventorySlot>();

                if (parentSlot != null && parentSlot.CanExchange(oldParentSlot))
                {
                    parentSlot.Exchange(oldParentSlot);
                }
                else if (slot != null && slot.CanExchange(oldParentSlot))
                {
                    slot.Exchange(oldParentSlot);
                }
                else
                {
                    // Return back to original position
                    GoToOldParent();
                }
            }
            else
            {
                // Return back to original position
                GoToOldParent();
            }
        }
        else
        {
            // Return back to original position
            GoToOldParent();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (item != null && item.name.Length > 0)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprites/" + item.name);

            if (sprite == null)
            {

                Debug.LogError("Sprite for " + item.name + " not found!");
                return;
            }

            this.image.sprite = sprite;
        }
    }

}
