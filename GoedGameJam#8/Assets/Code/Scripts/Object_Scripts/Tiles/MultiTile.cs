using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TileData/Machine Tile")]
public class MultiTile : ScriptableObject
{
    public Vector3Int size;

    public AnimatedTile[] multiTile;
}
