using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TileData/Machine Tile")]
public class MultiTile : ScriptableObject
{
    public string name;
    public Vector3Int size;

    public AnimatedTile[] multiTile;
}
