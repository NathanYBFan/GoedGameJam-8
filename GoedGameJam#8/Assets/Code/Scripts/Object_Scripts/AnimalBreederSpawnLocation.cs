using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBreederSpawnLocation : MonoBehaviour
{
    [SerializeField] private GameObject spawnAnimal;
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            SpawnAnimal();
        }
    }
    public void SpawnAnimal() {
        Vector3 temp = transform.position;
        temp.z = spawnAnimal.transform.position.z;
        Instantiate(spawnAnimal, temp, Quaternion.identity);
    }
    public GameObject GetSpawnAnimal() { return spawnAnimal; }
    public void SetSpawnAnimal(GameObject newAnimal) { spawnAnimal = newAnimal; }
}
