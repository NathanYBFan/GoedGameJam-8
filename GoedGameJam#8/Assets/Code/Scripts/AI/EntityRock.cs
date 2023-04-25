using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRock : MonoBehaviour
{
    [SerializeField] private float speedValue = 10;
    [SerializeField] private float maxRotation = 45;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, maxRotation * Mathf.Sin(Time.time * speedValue));
    }
}
