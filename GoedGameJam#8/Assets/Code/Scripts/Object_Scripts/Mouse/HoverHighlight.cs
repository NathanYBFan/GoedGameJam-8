using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;
public class HoverHighlight : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private SpriteEditorManager spriteEditorManager;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField, ReadOnly] private AnimatedTile[] hoverTile = null;
    [SerializeField] private Sprite defaultTile = null;
    [SerializeField] private Camera cam;
    private Vector3Int previousMousePos = new Vector3Int();
    Vector3Int gridPosition;

    void Update() {
        Vector3Int mousePos = GetMousePosition();
        gridPosition = mapManager.GetGameMap().WorldToCell(mousePos);

        if (!mousePos.Equals(previousMousePos)) {
            interactiveMap.ClearAllTiles(); // Remove all tiles
            if (hoverTile.Length == 1)
                interactiveMap.SetTile(mousePos, hoverTile[0]);
            else
                SetTilesExtractorHover();

            // Color change if can place
            
            ResetTileMapColor();
            previousMousePos = mousePos;
        }
    }
    public void ResetTileMapColor() {
        if (mapManager.GetSelectedAnimatedTile() != null && mapManager.GetMachineMap().GetTile(gridPosition) != null)
                interactiveMap.color = GetNewTileMapColor(Color.red);
            else if (mapManager.GetSelectedMultiTile() != null && mapManager.GetMachineMap().GetTile(gridPosition) != null)
                interactiveMap.color = GetNewTileMapColor(Color.red);
            else if (mapManager.GetSelectedMultiTile() != null && mapManager.GetSelectedMultiTile().name == "Extractor" && !CanPlaceExtractor())
                interactiveMap.color = GetNewTileMapColor(Color.red);
            else
                interactiveMap.color = GetNewTileMapColor(Color.green);
    }
    private bool CanPlaceExtractor() {
        if (mapManager.GetMachineMap().GetTile(gridPosition) != null) return false;
        
        Vector3Int tempTilePos = gridPosition;
        for (int x = 0; x < 2; x++) {
            for (int y = 0; y < 3; y++) {
                tempTilePos.x = gridPosition.x + x;
                tempTilePos.y = gridPosition.y + y;
                if (mapManager.GetMachineMap().GetTile(tempTilePos) != null) return false;
                if (mapManager.GetConveyorMap().GetTile(tempTilePos) != null) return false;
            }
        }
        return true;
    }
    private Color GetNewTileMapColor(Color colorToAdd) {
        Color newColor = colorToAdd;
        newColor.a = 0.5f;
        return newColor;
    }
    private Vector3Int GetMousePosition() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mapManager.GetGameMap().WorldToCell(mouseWorldPos);
    }
    
    public void SetMultiHoverDisplay(MultiTile selectedTile) { // Should not be used
        if (selectedTile == null)
            SetRuleHoverDisplay(null);
        else {
            hoverTile = selectedTile.directedTile;
            interactiveMap.ClearAllTiles();
            
            SetTilesExtractorHover();
        }
    }

    private void SetTilesExtractorHover() {
        int counter = 0;
            for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
                for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                    Tile newTile = (Tile) ScriptableObject.CreateInstance(typeof(Tile));
                    newTile.sprite = mapManager.GetSelectedMultiTile().directedTile[counter].m_AnimatedSprites[spriteEditorManager.GetSelectedTile()];
                    if (spriteEditorManager.GetSelectedTile() == 0)
                        interactiveMap.SetTile(gridPosition + new Vector3Int(j, i, 0), newTile);
                    else if (spriteEditorManager.GetSelectedTile() == 1)
                        interactiveMap.SetTile(gridPosition + new Vector3Int(j, i, 0), newTile);
                    else if (spriteEditorManager.GetSelectedTile() == 2)
                        interactiveMap.SetTile(gridPosition + new Vector3Int(j, i, 0), newTile);
                    else if (spriteEditorManager.GetSelectedTile() == 3)
                        interactiveMap.SetTile(gridPosition + new Vector3Int(j, i, 0), newTile);
                    counter++;
                }
            }
    }

    public void SetRuleHoverDisplay(RuleTile selectedTile) {
        if (selectedTile == null) {
            hoverTile = MakeNewTile(defaultTile);
            interactiveMap.SetTile(gridPosition, hoverTile[0]);
            interactiveMap.color = GetNewTileMapColor(Color.green);
        }
        else
            hoverTile = MakeNewTile(selectedTile.m_DefaultSprite);
    }
    public void SetAnimHoverDisplay(AnimatedTile selectedTile) {
        if (selectedTile == null)
            SetRuleHoverDisplay(null);
        else
            hoverTile = MakeNewTile(selectedTile.m_AnimatedSprites[0]);
        interactiveMap.SetTile(GetMousePosition(), selectedTile);
        
    }

    private AnimatedTile[] MakeNewTile(Sprite newSprite) {
        Sprite[] arrayOfSprites = new Sprite[1];
        arrayOfSprites[0] = newSprite;
        AnimatedTile newTile = (AnimatedTile) ScriptableObject.CreateInstance<AnimatedTile>();
        newTile.m_AnimatedSprites = arrayOfSprites;
        AnimatedTile[] returnType = new AnimatedTile[1];
        returnType[0] = newTile;
        return returnType;
    }
}
