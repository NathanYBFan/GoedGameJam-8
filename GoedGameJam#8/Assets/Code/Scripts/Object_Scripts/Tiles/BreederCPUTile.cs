using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Breeder Controller Tile")]
public class BreederCPUTile : Tile
{
    public ContainerTile input1, input2;
    public OutputTile output;
    public List<Recipe> animalRecipes;
    public Recipe currentRecipe;

    public bool CheckForValidRecipes()
    {
        bool hasValidRecipe = false;
        if (input1.heldItem == null && input2.heldItem == null) return false;
        foreach (Recipe r in animalRecipes)
        {
            if (input1.heldItem == null)
            {
                if (input2.heldItem.GetComponent<Item>().itemType == r.firstIngredient || input2.heldItem.GetComponent<Item>().itemType == r.secondIngredient)
                {
                    currentRecipe = r;
                    output.spawnItem = r.output;               
                    Destroy(input2.heldItem);       
                    input2.heldItem = null;
                    return true;
                }
            }
            if (input2.heldItem == null)
            {
                if (input1.heldItem.GetComponent<Item>().itemType == r.firstIngredient || input1.heldItem.GetComponent<Item>().itemType == r.secondIngredient)
                {
                    currentRecipe = r;
                    output.spawnItem = r.output;               
                    Destroy(input1.heldItem);       
                    input1.heldItem = null;
                    return true;
                }
            }
            if (input1.heldItem != null && input2.heldItem != null)
            {
                if ((input1.heldItem.GetComponent<Item>().itemType == r.firstIngredient || input1.heldItem.GetComponent<Item>().itemType == r.secondIngredient) || (input2.heldItem.GetComponent<Item>().itemType == r.firstIngredient || input2.heldItem.GetComponent<Item>().itemType == r.secondIngredient))
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
            

        }
        if (input1.heldItem != null)
            Destroy(input1.heldItem);   
        if (input2.heldItem != null)               
            Destroy(input2.heldItem);    
        input1.heldItem = null;
        input2.heldItem = null;
        return false;

    }

}
