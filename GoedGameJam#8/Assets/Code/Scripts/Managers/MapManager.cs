using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;
public class MapManager : MonoBehaviour
{
    [Header("Map manager Info")]
    [SerializeField, ReadOnly] private RuleTile selectedRuleTile;
    [SerializeField, ReadOnly] private AnimatedTile selectedAnimatedTile;
    [SerializeField] private List<GameTileData> gameTileDatas;
    [SerializeField] HoverHighlight hoverHighlight;

    [Header("Maps")]
    [SerializeField] private Tilemap gameMap;
    [SerializeField] private Tilemap conveyorMap;
    [SerializeField] private Tilemap machineMap;
    private Dictionary<TileBase, GameTileData> dataFromTiles;

    private void Awake() {
        dataFromTiles = new Dictionary<TileBase, GameTileData>();

        foreach(GameTileData gameTileData in gameTileDatas) {
            dataFromTiles.Add(gameTileData.tiles, gameTileData);
        }
    }

    public RuleTile GetSelectedRuleTile() { return selectedRuleTile; }
    public void SetSelectedRuleTile(RuleTile newTile) {
        selectedRuleTile = newTile;
        selectedAnimatedTile = null;
        hoverHighlight.SetRuleHoverDisplay(newTile);
    }
    public AnimatedTile GetSelectedAnimatedTile() { return selectedAnimatedTile; }
    public void SetSelectedAnimatedTile(AnimatedTile newSelectedTile) {
        selectedAnimatedTile = newSelectedTile;
        selectedRuleTile = null; 
        hoverHighlight.SetAnimHoverDisplay(newSelectedTile);
    }

    public Enums.TileTypes GetTileType(TileBase tilebase) { return dataFromTiles[tilebase].tileType; }
    public bool GetTileHasMachine(TileBase tilebase) { return dataFromTiles[tilebase].hasMachineOntop; }
    public Tilemap GetGameMap() { return gameMap; }
    public Tilemap GetConveyorMap() { return conveyorMap; }
    public Tilemap GetMachineMap() { return machineMap; }
    
}
