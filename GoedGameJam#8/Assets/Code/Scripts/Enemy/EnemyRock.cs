using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRock : MonoBehaviour
{
    public float speedValue = 10;
    public float maxRotation = 45;
    public GameObject pivot;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, maxRotation * Mathf.Sin(Time.time * speedValue));
       
    }
}
