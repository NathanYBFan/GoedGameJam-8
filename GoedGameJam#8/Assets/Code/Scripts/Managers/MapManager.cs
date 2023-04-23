using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Header("Map manager Info")]
    [SerializeField] private Tile selectedTile;
    [SerializeField] private List<TileData> tileDatas;
    [SerializeField] HoverHighlight hoverHighlight;

    [Header("Maps")]
    [SerializeField] private Tilemap gameMap;
    [SerializeField] private Tilemap machineMap;
    [SerializeField] private Tilemap mouseHover;
    private Dictionary<TileBase, TileData> dataFromTiles;

    private void Awake() {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas) {
            foreach (var tile in tileData.tiles) {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    public Tile GetSelectedTile() { return selectedTile; }
    public void SetSelectedTile(Tile newTile) {
        selectedTile = newTile;
        hoverHighlight.SetHoverDisplay(newTile);
    }

    public Enums.TileTypes GetTileType(TileBase tilebase) { return dataFromTiles[tilebase].tileType; }
    public Tilemap GetGameMap() { return gameMap; }
    public Tilemap GetMachineMap() { return machineMap; }
}
