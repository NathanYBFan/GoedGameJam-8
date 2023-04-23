using UnityEngine;

[CreateAssetMenu(menuName = "TileData/Environment")]
public class EnvironmentTileData : ScriptableObject
{
    public RuleTile[] tiles;
    public bool canMine;
    public bool hasMachineOntop;
    public bool isLand;
    public Enums.TileTypes tileType;
}
