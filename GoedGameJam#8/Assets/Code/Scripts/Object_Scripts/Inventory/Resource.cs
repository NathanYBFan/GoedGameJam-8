using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Inventory/Resource")]
public class Resource : ScriptableObject
{    
    public Sprite icon;
    new public string name = "";
    public int amount;
}
