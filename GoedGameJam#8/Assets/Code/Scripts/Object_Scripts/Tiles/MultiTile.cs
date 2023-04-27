using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TileData/Machine Tile")]
public class MultiTile : ScriptableObject
{
    public new string name;
    public Vector3Int size;
    public AnimatedTile[] directedTile;
    public Vector3Int localOutputLocation;
    public GameObject spawnLocation;

    void Start()
    {
        
    }

}
