using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public MapManager mapManager;
    public RuleTile barrenTile;
    public List<Vector3> dirtSpawners = new();
    public List<Vector3> soilSpawners = new();
    public List<Vector3> waterSpawners = new();
    
    public List<Vector3> grassSpawners = new();
    
    public List<Vector3> seedSpawners = new();
    public List<Vector3> wheatSpawners = new();
    public List<Vector3> flowerSpawners = new();

    public List<Recipe> recipes = new();
    
    public List<Recipe> animalRecipes = new();
    
    public List<Recipe> incineratingRecipes = new();

    public List<CombinerCPUTile> combiners = new();
    public List<BreederCPUTile> breeders = new();
    
    public List<IncineratorTile> incinerators = new();
    
    public List<UploaderTile> uploaders = new();
    public GameObject[] spawnItems;
    public float spawnInterval = 2.0f;
    
    public void Awake()
    {
        InvokeRepeating("SpawnResource", 0, 2.0f);
    }
    void Update()
    {
        CheckCombiners();
        CheckBreeders();
        CheckIncinerators();            
        CheckUploaders();    
        CheckSpawners();  
    }
    void CheckSpawners()
    {
        if (wheatSpawners.Count > 0) {
            List<Vector3> toRemove = new();
            foreach (Vector3 v in wheatSpawners) {                
                if (mapManager.GetPlantMap().GetTile(Vector3Int.FloorToInt(v)) == null) {
                    toRemove.Add(v);
                    ReAssignSpawner(v, mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(v)).name);
                }
            }
            foreach(Vector3 v in toRemove)
                wheatSpawners.Remove(v);
        }
        if (flowerSpawners.Count > 0) {
            List<Vector3> toRemove = new();
            foreach (Vector3 v in flowerSpawners) {
                if (mapManager.GetPlantMap().GetTile(Vector3Int.FloorToInt(v)) == null) {
                    toRemove.Add(v);
                    ReAssignSpawner(v, mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(v)).name);
                }
            }
            foreach(Vector3 v in toRemove)
                flowerSpawners.Remove(v);
        }
        if (seedSpawners.Count > 0) {
            List<Vector3> toRemove = new();
            foreach (Vector3 v in seedSpawners) {
                string name = mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(v)).name;
                if (!name.Contains("Seed")) {
                    toRemove.Add(v);
                    ReAssignSpawner(v, name);
                }
            }
            foreach(Vector3 v in toRemove)
                seedSpawners.Remove(v);
        }
        if (dirtSpawners.Count > 0) {
            List<Vector3> toRemove = new();
            foreach (Vector3 v in dirtSpawners) {
                string name = mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(v)).name;
                if (!name.Contains("Barren")) {
                    toRemove.Add(v);
                    ReAssignSpawner(v, name);
                }
            }
            foreach(Vector3 v in toRemove)
                seedSpawners.Remove(v);
        }
        if (soilSpawners.Count > 0) {
            List<Vector3> toRemove = new();
            foreach (Vector3 v in soilSpawners) {
                string name = mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(v)).name;
                if (!name.Contains("Soil")) {
                    toRemove.Add(v);
                    ReAssignSpawner(v, name);
                }
            }
            foreach(Vector3 v in toRemove)
                seedSpawners.Remove(v);
        }
        if (waterSpawners.Count > 0) {
            List<Vector3> toRemove = new();
            foreach (Vector3 v in waterSpawners) {
                string name = mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(v)).name;
                if (!name.Contains("Water")) {
                    toRemove.Add(v);
                    ReAssignSpawner(v, name);                                  
                }
            }
            foreach(Vector3 v in toRemove)
                seedSpawners.Remove(v);
        }
        if (grassSpawners.Count > 0) {
            List<Vector3> toRemove = new();
            foreach (Vector3 v in grassSpawners) {
                string name = mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(v)).name;
                if (!name.Contains("Grass")) {
                    toRemove.Add(v);
                    ReAssignSpawner(v, name);
                }
            }
            foreach(Vector3 v in toRemove)
                seedSpawners.Remove(v);
        }
    }

    private void ReAssignSpawner(Vector3 v, string name) {
        if (name.Contains("Seed") && !seedSpawners.Contains(v)) seedSpawners.Add(v);    
        if (name.Contains("Wheat") && !wheatSpawners.Contains(v)) wheatSpawners.Add(v);    
        if (name.Contains("Flower") && !flowerSpawners.Contains(v)) flowerSpawners.Add(v);       
        if (name.Contains("Barren") && !dirtSpawners.Contains(v)) dirtSpawners.Add(v);                     
        if (name.Contains("Soil") && !soilSpawners.Contains(v)) soilSpawners.Add(v);                                    
        if (name.Contains("Water") && !waterSpawners.Contains(v)) waterSpawners.Add(v);
        if (name.Contains("Grass") && !grassSpawners.Contains(v)) grassSpawners.Add(v);          
    }

    public void SpawnResource()
    {
        SpawnItems(dirtSpawners, 0);
        SpawnItems(soilSpawners, 1);
        SpawnItems(waterSpawners, 2);        
        SpawnItems(grassSpawners, 3);
        SpawnSeeds(seedSpawners);        
        SpawnPlants(wheatSpawners);             
        SpawnPlants(flowerSpawners);
    }

    private void SpawnPlants(List<Vector3> resource)
    {
        foreach(Vector3 t in resource){
            if (mapManager.GetPlantMap().GetTile(Vector3Int.FloorToInt(t)) is SeedTile seedTile) 
            {
                int itemToSpawn = seedTile.randSeed;
                Vector3 temp = t;
                temp.z = spawnItems[itemToSpawn].transform.position.z;
                Instantiate(spawnItems[itemToSpawn], temp, Quaternion.identity);
                seedTile.seedsAmount--;

                if (seedTile.seedsAmount <= 0)
                {                    
                    mapManager.GetPlantMap().SetTile(Vector3Int.FloorToInt(t), null);
                }
            }  
        }
    }

    private void SpawnSeeds(List<Vector3> resource)
    {
        foreach(Vector3 t in resource){
            if (mapManager.GetGameMap().GetTile(Vector3Int.FloorToInt(t)) is SeedTile seedTile) 
            {
                int itemToSpawn = seedTile.randSeed;
                Vector3 temp = t;
                temp.z = spawnItems[itemToSpawn].transform.position.z;
                Instantiate(spawnItems[itemToSpawn], temp, Quaternion.identity);
                seedTile.seedsAmount--;

                if (seedTile.seedsAmount <= 0)
                {                    
                    mapManager.GetGameMap().SetTile(Vector3Int.FloorToInt(t), barrenTile);
                }
            }  
        }
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
            if (c.input1.heldItem != null && c.input2.heldItem != null)
            {
                if (c.CheckForValidRecipes())
                {
                    c.output.OutputItem();
                }
            }
        }
    }
    private void CheckBreeders()
    {
        foreach(BreederCPUTile b in breeders)
        {
            if (b.input1.heldItem != null || b.input2.heldItem != null)
            {
                if (b.CheckForValidRecipes())
                {
                    b.output.OutputItem();
                }
            }
        }
    }
    private void CheckIncinerators()
    {
        foreach(IncineratorTile c in incinerators)
        {
            if (c.heldItem != null)
            {
                c.CheckForValidIncineration();
                Destroy(c.heldItem);
                
            }
        }
    }
    private void CheckUploaders()
    {
        foreach(UploaderTile c in uploaders)
        {
            if (c.heldItem != null)
            {
                c.StoreItem();
                Destroy(c.heldItem);
                
            }
        }
    }
}
