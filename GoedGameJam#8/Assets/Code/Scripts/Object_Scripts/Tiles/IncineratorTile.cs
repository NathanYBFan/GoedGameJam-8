using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Incinerator Tile")]
public class IncineratorTile : Tile
{
    public Vector2 movementDir;
    public Vector3 position;
    public GameObject heldItem;
    public GameObject spawnItem;
    public List<Recipe> recipes;
    public Recipe currentRecipe;
    

    public bool CheckForValidIncineration()
    {

        bool isValid = false;
        if (heldItem == null) return false;        
        foreach (Recipe r in recipes)
        {
            if ((heldItem.GetComponent<Item>().itemType == r.firstIngredient))
            {
                currentRecipe = r;
                spawnItem = r.output;
                OutputItem();
                Destroy(heldItem);                       
                heldItem = null;
                return true;
            }

        }
        Destroy(heldItem);                       
        heldItem = null;
        return false;

    }
    public void OutputItem()
    {
        Instantiate(spawnItem, position, Quaternion.identity);
        spawnItem = null;
    }
}
