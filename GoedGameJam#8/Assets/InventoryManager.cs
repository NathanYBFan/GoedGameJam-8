using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Resource> inventory = new();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Resource r in inventory)
        {
            r.amount = 0;
        }
    }
    public void AddToInventory(Item item, int amount)
    {
        foreach (Resource r in inventory)
        {
            if (r.tileType == item.itemType)
            {
                r.amount += amount;
                return;
            }
        }

    }
    public void RemoveFromInventory(Item item)
    {
        foreach (Resource r in inventory)
        {
            if (r.tileType == item.itemType)
            {
                if (r.amount <= 0) return;

                
                r.amount -= 1;
                return;
            }
        }

    }
}
