using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TileData/Conveyors")]
public class ConveyorTiles : ScriptableObject
{
    public AnimatedTile animTile;
    public Enums.Directions directionOfMovement;
}
