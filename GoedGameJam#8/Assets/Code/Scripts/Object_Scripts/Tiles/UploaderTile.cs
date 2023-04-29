using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Uploader Tile")]
public class UploaderTile : Tile
{
    public GameObject heldItem;
    public InventoryManager resourceManager;   

    
    public void StoreItem()
    {
        if (heldItem == null) return;
        
        foreach (Resource r in resourceManager.inventory)
        {
            if (r.tileType == heldItem.GetComponent<Item>().itemType)
            {
                resourceManager.AddToInventory(heldItem.GetComponent<Item>(), 1);
                Destroy(heldItem);
                heldItem = null;
                return;
            }
        }
        Destroy(heldItem);
        heldItem = null;
    }
}
