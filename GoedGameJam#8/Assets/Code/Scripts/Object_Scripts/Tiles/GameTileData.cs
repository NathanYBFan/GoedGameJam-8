using UnityEngine;

[CreateAssetMenu(menuName = "TileData/TileData")]
public class GameTileData : ScriptableObject
{
    public RuleTile tiles;
    public bool canMine;
    public bool isLand;
    public bool hasMachineOntop;
    public Enums.TileTypes tileType = Enums.TileTypes.environment;
}
