using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Output Tile")]
public class OutputTile : Tile
{
    public GameObject spawnItem;
    public Vector3 position;
    public Vector2 movementDir;

    public void OutputItem()
    {
        Instantiate(spawnItem, position, Quaternion.identity);
        spawnItem = null;
    }
}
