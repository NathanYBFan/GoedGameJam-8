using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Animal : MonoBehaviour
{
    private float hunger;
    private float life;

    protected abstract void Act();
    
    public void TakeDamage(float dmg)
    {
        life -= dmg;
    }

    public void Die() //might make virtual so can be overwritten
    {
        Destroy(this.gameObject);
    }
}
