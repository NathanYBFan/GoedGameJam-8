using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public List<Recipe> recipes;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Recipe r in recipes)
        {
            r.inventory = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
