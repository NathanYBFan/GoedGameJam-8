using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;
public class MapManager : MonoBehaviour
{
    [Header("Map manager Info")]
    [SerializeField, ReadOnly] private RuleTile selectedRuleTile;
    [SerializeField, ReadOnly] private AnimatedTile selectedAnimatedTile;
    [SerializeField, ReadOnly] private MultiTile selectedMultiTile;
    [SerializeField] private List<GameTileData> gameTileDatas;
    [SerializeField] private List<ConveyorTiles> conveyorAnimatedTiles;
    [SerializeField] private HoverHighlight hoverHighlight;

    [Header("Maps")]
    [SerializeField] private Tilemap gameMap;
    [SerializeField] private Tilemap conveyorMap;
    [SerializeField] private Tilemap machineMap;
    [SerializeField] private Tilemap hoverMap;
    private Dictionary<TileBase, GameTileData> dataFromTiles;
    private static Dictionary<Sprite, ConveyorTiles> conveyorDataTiles; // unstatic later on
    private void Awake() {
        dataFromTiles = new Dictionary<TileBase, GameTileData>();
        conveyorDataTiles = new Dictionary<Sprite, ConveyorTiles>();

        foreach(GameTileData gameTileData in gameTileDatas) {
            dataFromTiles.Add(gameTileData.tiles, gameTileData);
        }

        foreach(ConveyorTiles animatedTile in conveyorAnimatedTiles) {
            foreach(Sprite sprite in animatedTile.animTile.m_AnimatedSprites)
                conveyorDataTiles.Add(sprite, animatedTile);
        }
    }

    public RuleTile GetSelectedRuleTile() { return selectedRuleTile; }
    public void SetSelectedRuleTile(RuleTile newTile) {
        ResetSelectedTiles();
        selectedRuleTile = newTile;
        hoverHighlight.SetRuleHoverDisplay(newTile);
    }
    public MultiTile GetSelectedMultiTile() { return selectedMultiTile; }
    public void SetSelectedMultiTile(MultiTile multiTile) {
        ResetSelectedTiles();
        selectedMultiTile = multiTile;
        hoverHighlight.SetMultiHoverDisplay(multiTile);
    }
    public AnimatedTile GetSelectedAnimatedTile() { return selectedAnimatedTile; }
    public void SetSelectedAnimatedTile(AnimatedTile newSelectedTile) {
        ResetSelectedTiles();
        selectedAnimatedTile = newSelectedTile;
        hoverHighlight.SetAnimHoverDisplay(newSelectedTile);
    }

    public Enums.TileTypes GetTileType(TileBase tilebase) { return dataFromTiles[tilebase].tileType; }
    public static Dictionary<Sprite, ConveyorTiles> GetConveyorDictionary() { return conveyorDataTiles; } 

    public Tilemap GetGameMap() { return gameMap; }
    public Tilemap GetConveyorMap() { return conveyorMap; }
    public Tilemap GetMachineMap() { return machineMap; }
    public Tilemap GetHoverMap() { return hoverMap; }
    private void ResetSelectedTiles() {
        selectedAnimatedTile = null;
        selectedMultiTile = null;
        selectedRuleTile = null;
    }    
}
