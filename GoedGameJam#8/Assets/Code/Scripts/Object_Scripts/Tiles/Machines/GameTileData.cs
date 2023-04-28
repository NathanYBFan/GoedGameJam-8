using UnityEngine;

[CreateAssetMenu(menuName = "TileData/TileData")]
public class GameTileData : ScriptableObject
{
    public RuleTile tiles;
    public bool canMine;
    public bool isLand;
    public Enums.TileTypes tileType = Enums.TileTypes.environment;
}
