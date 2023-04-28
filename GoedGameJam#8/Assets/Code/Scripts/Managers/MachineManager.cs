using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public List<Vector3> dirtSpawners = new();
    public List<Vector3> soilSpawners = new();
    public List<Vector3> waterSpawners = new();
    public GameObject[] spawnItems;
    public float spawnInterval = 2.0f;
    
    public void Awake()
    {
        InvokeRepeating("SpawnResource", 0, 2.0f);
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
}
