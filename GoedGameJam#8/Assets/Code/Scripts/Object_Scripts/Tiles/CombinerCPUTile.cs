using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Combiner Controller Tile")]
public class CombinerCPUTile : Tile
{
    public ContainerTile input1, input2;
    public OutputTile output;
    public List<Recipe> recipes;
    public Recipe currentRecipe;

    public bool CheckForValidRecipes()
    {
        bool hasValidRecipe = false;
        if (input1.heldItem == null || input2.heldItem == null) return false;
        foreach (Recipe r in recipes)
        {
            if ((input1.heldItem.GetComponent<Item>().itemType == r.firstIngredient && input2.heldItem.GetComponent<Item>().itemType == r.secondIngredient) || (input2.heldItem.GetComponent<Item>().itemType == r.firstIngredient && input1.heldItem.GetComponent<Item>().itemType == r.secondIngredient))
            {
                currentRecipe = r;
                output.spawnItem = r.output;
                Destroy(input1.heldItem);                  
                Destroy(input2.heldItem);               
                input1.heldItem = null;
                input2.heldItem = null;
                return true;
            }

        }
        Destroy(input1.heldItem);                  
        Destroy(input2.heldItem);    
        input1.heldItem = null;
        input2.heldItem = null;
        return false;

    }

}
