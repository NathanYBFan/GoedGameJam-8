using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tile selectedTile;
    [SerializeField] private List<TileData> tileDatas;
    [SerializeField] HoverHighlight hoverHighlight;
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

    public TileData GetDictionary(TileBase tilebase) { return dataFromTiles[tilebase]; }
}
