using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TileData/Machine Tile")]
public class MultiTile : ScriptableObject
{
    public new string name;
    public Vector3Int size;

    public AnimatedTile[] facingUpTile;
    public AnimatedTile[] facingRightTile;
    public AnimatedTile[] facingDownTile;
    public AnimatedTile[] facingLeftTile;
}
