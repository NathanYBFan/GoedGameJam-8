using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public List<Vector3> dirtSpawners = new();
    public List<Vector3> soilSpawners = new();
    public List<Vector3> waterSpawners = new();

    public List<Recipe> recipes = new();

    public List<CombinerCPUTile> combiners = new();
    public GameObject[] spawnItems;
    public float spawnInterval = 2.0f;
    
    public void Awake()
    {
        InvokeRepeating("SpawnResource", 0, 2.0f);
    }
    void Update()
    {
        CheckCombiners();
    }
    public void SpawnResource()
    {
        SpawnItems(dirtSpawners, 0);
        SpawnItems(soilSpawners, 1);
        SpawnItems(waterSpawners, 2);
    }

    private void SpawnItems(List<Vector3> resource, int itemToSpawn) {
        foreach(Vector3 t in resource) {
            Vector3 temp = t;
            temp.z = spawnItems[itemToSpawn].transform.position.z;
            Instantiate(spawnItems[itemToSpawn], temp, Quaternion.identity);
        }
    }

    private void CheckCombiners()
    {
        foreach(CombinerCPUTile c in combiners)
        {
            if (c.input1.heldItem != null & c.input2.heldItem != null);
            {
                if (c.CheckForValidRecipes())
                {
                    c.output.OutputItem();
                }
            }
        }
    }
}
