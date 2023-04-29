using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe")]
public class Recipe : ScriptableObject
{    
    public Enums.ItemType firstIngredient, secondIngredient;
    public GameObject output;   
    public Resource tileResource;   
    public int outputAmount = 0;
    void Awake()
    {
        
    }  
    /*
    public void OnRecipeSelected()
    {
        if (HasEnoughResources())
        {
            foreach(Resource r in inventory.resources)
            {
                if (r.name == firstIngredient.name) r.amount -= firstIngredientAmount;                
                if (r.name == secondIngredient.name) r.amount -= secondIngredientAmount;
                if (r.name == output.name) r.amount += outputAmount;
            }
        }
    }
    //Probably lol. I think the methods would functionally do the same though. Idk fs
    public bool HasEnoughResources()
    {
        bool hasEnough = true;
        foreach(Resource r in inventory.resources)
        {
            if (r.name == firstIngredient.name)
            {
                if (r.amount < firstIngredientAmount)
                {
                    hasEnough = false;
                    break;
                }
            }
            if (r.name == secondIngredient.name)
            {
                if (r.amount < secondIngredientAmount)
                {
                    hasEnough = false;
                    break;
                }
            }
        }
        return hasEnough;
    }*/
}
