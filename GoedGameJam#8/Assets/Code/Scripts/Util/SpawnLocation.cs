using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    public GameObject spawnItem;
    public float spawnInterval = 2.0f;
    
    public void Awake()
    {
        InvokeRepeating("SpawnResource", 0, 2.0f);
    }
    public void SpawnResource()
    {
        Instantiate(spawnItem, transform.position, Quaternion.identity);
    }
}
