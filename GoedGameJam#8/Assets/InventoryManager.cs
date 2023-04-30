using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryManager : MonoBehaviour
{
    public List<Resource> inventory = new();    
    [SerializeField] private TextMeshProUGUI grassButton, waterButton, dirtButton, soilButton, flowerButton, wheatButton;
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
    public void RemoveFromInventory(Enums.ItemType itemType)
    {
        foreach (Resource r in inventory)
        {
            if (r.tileType == itemType)
            {
                if (r.amount <= 0) return;

                r.amount -= 1;
                return;
            }
        }

    }
    void Update()
    {
        flowerButton.text = "Flower\nx" + inventory[0].amount;
        grassButton.text = "Grass\nx" + inventory[1].amount;
        dirtButton.text = "Dirt\nx" + inventory[2].amount;
        soilButton.text = "Soil\nx" + inventory[3].amount;
        waterButton.text = "Water\nx" + inventory[4].amount;
        wheatButton.text = "Wheat\nx" + inventory[5].amount;
    }
}
