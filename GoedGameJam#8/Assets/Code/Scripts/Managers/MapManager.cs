using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Header("Map manager Info")]
    [SerializeField] private RuleTile selectedTile;
    [SerializeField] private List<EnvironmentTileData> environmentTileDatas;
    [SerializeField] HoverHighlight hoverHighlight;

    [Header("Maps")]
    [SerializeField] private Tilemap gameMap;
    [SerializeField] private Tilemap machineMap;
    [SerializeField] private Tilemap mouseHover;
    private Dictionary<RuleTile, EnvironmentTileData> dataFromTiles;

    private void Awake() {
        dataFromTiles = new Dictionary<RuleTile, EnvironmentTileData>();

        foreach(EnvironmentTileData tileData in environmentTileDatas) {
            foreach (RuleTile tile in tileData.tiles) {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    public RuleTile GetSelectedTile() { return selectedTile; }
    public void SetSelectedTile(RuleTile newTile) {
        selectedTile = newTile;
        hoverHighlight.SetHoverDisplay(newTile);
    }

    public Enums.TileTypes GetTileType(RuleTile tilebase) { return dataFromTiles[tilebase].tileType; }
    public Tilemap GetGameMap() { return gameMap; }
    public Tilemap GetMachineMap() { return machineMap; }
}
