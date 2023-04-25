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
    [SerializeField] private List<ConveyorTiles> conveyorAnimatedTiles;
    [SerializeField] HoverHighlight hoverHighlight;

    [Header("Maps")]
    [SerializeField] private Tilemap gameMap;
    [SerializeField] private Tilemap conveyorMap;
    [SerializeField] private Tilemap machineMap;
    private Dictionary<TileBase, GameTileData> dataFromTiles;
    private static Dictionary<Sprite, ConveyorTiles> conveyorDataTiles;
    private void Awake() {
        dataFromTiles = new Dictionary<TileBase, GameTileData>();
        conveyorDataTiles = new Dictionary<Sprite, ConveyorTiles>();

        foreach(GameTileData gameTileData in gameTileDatas) {
            dataFromTiles.Add(gameTileData.tiles, gameTileData);
        }

        foreach(ConveyorTiles animatedTile in conveyorAnimatedTiles) {
            foreach(Sprite sprite in animatedTile.animTile.m_AnimatedSprites) {
                conveyorDataTiles.Add(sprite, animatedTile);
                Debug.Log(sprite.name);
            }
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
    public static Dictionary<Sprite, ConveyorTiles> GetConveyorDictionary() { return conveyorDataTiles; } 

    public Tilemap GetGameMap() { return gameMap; }
    public Tilemap GetConveyorMap() { return conveyorMap; }
    public Tilemap GetMachineMap() { return machineMap; }
    
}
