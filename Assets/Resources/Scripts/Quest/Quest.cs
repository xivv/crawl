using System.Collections.Generic;
using UnityEngine;

public class Quest : Identifier
{

    public string name;
    public string description;
    public List<int> requisites;
    public List<Item> requiredItems = new List<Item>();
    public List<Item> loot = new List<Item>();
    public bool done = false;

    public bool IsSuccessFull(List<Item> inventory)
    {

        foreach (Item item in requiredItems)
        {

            bool contains = false;

            foreach (Item inventoryItem in inventory)
            {
                if (item.name == inventoryItem.name)
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                return false;
            }
        }

        return true;
    }

    public List<Item> OnFailure()
    {
        done = false;
        Debug.Log("Quest " + id + " not successfull");

        return requiredItems;
    }

    public List<Item> OnSuccess()
    {
        done = true;
        Debug.Log("Quest " + id + " successfull");

        return loot;
    }
}
