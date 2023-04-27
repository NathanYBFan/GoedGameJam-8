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
        foreach(Vector3 t in dirtSpawners)
        {            
            Instantiate(spawnItems[0], t, Quaternion.identity);
        }
        foreach(Vector3 t in soilSpawners)
        {            
            Instantiate(spawnItems[1], t, Quaternion.identity);
        }
        foreach(Vector3 t in waterSpawners)
        {            
            Instantiate(spawnItems[2], t, Quaternion.identity);
        }
    }
}
