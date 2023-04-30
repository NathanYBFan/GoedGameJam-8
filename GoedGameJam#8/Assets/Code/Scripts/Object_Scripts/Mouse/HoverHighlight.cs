using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;
public class HoverHighlight : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private SpriteEditorManager spriteEditorManager;
    
    [SerializeField] private InventoryManager resourceManager;
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

            if (hoverTile.Length == 1) {
                if (hoverTile[0] != null && hoverTile[0].name == "Incinerator" || hoverTile[0] != null && hoverTile[0].name == "Uploader")
                    interactiveMap.SetTile(mousePos, MakeTile(hoverTile[0].m_AnimatedSprites[spriteEditorManager.GetSelectedTile()]));
                else
                    interactiveMap.SetTile(mousePos, hoverTile[0]);
            }
            else
                SetTilesMachineHover();

            // Color change if can place
            ResetTileMapColor();
            previousMousePos = mousePos;
        }
    }
    public bool HasResource()
    {
        string name = mapManager.GetSelectedRuleTile().name;
        if (name.Contains("Barren"))
        {
            return CheckInventory(Enums.ItemType.Dirt);
        }
        else if (name.Contains("Water"))
        {
            return CheckInventory(Enums.ItemType.Water);
        }
        else if (name.Contains("Soil"))
        {
            return CheckInventory(Enums.ItemType.Soil);
        }
        else if (name.Contains("Grass"))
        {
            return CheckInventory(Enums.ItemType.GrassSeeds);
        }

        return false;
    }
    public bool CanSeedBePlaced()
    {
        string name = mapManager.GetSelectedSeedTile().name;
        if (name.Contains("Wheat"))
        {
            if (!CheckInventory(Enums.ItemType.WheatSeeds)) return false;
            if (!mapManager.GetGameMap().GetTile(gridPosition).name.Contains("Soil")) return false;
        }
        else if (name.Contains("Flower"))
        {            
            if (!CheckInventory(Enums.ItemType.FlowerSeeds)) return false;
            if (!mapManager.GetGameMap().GetTile(gridPosition).name.Contains("Soil") && !mapManager.GetGameMap().GetTile(gridPosition).name.Contains("Grass")) return false;
        }
        return true;
    }
    private bool CheckInventory(Enums.ItemType itemType)
    {
        foreach (Resource r in resourceManager.inventory)
        {
            if (r.tileType == itemType)
            {
                if (r.amount <= 0) return false;
                else return true;
            }
        }
        return false;
    }
    public void ResetTileMapColor() {
        if (mapManager.GetSelectedAnimatedTile() != null && mapManager.GetMachineMap().GetTile(gridPosition) != null)
            interactiveMap.color = GetNewTileMapColor(Color.red);
        else if (mapManager.GetSelectedMultiTile() != null && mapManager.GetMachineMap().GetTile(gridPosition) != null)
            interactiveMap.color = GetNewTileMapColor(Color.red);
        else if (mapManager.GetSelectedMultiTile() != null && mapManager.GetSelectedMultiTile().name == "Extractor" && !CanPlaceMachine())
            interactiveMap.color = GetNewTileMapColor(Color.red);
        else if (mapManager.GetSelectedRuleTile() != null && !HasResource())
            interactiveMap.color = GetNewTileMapColor(Color.red);
        else if (mapManager.GetSelectedSeedTile() != null && !CanSeedBePlaced())
            interactiveMap.color = GetNewTileMapColor(Color.red);
        else
            interactiveMap.color = GetNewTileMapColor(Color.green);
    }

    // Check if the current selected machine can be placed; Are there any overlapping tiles
    private bool CanPlaceMachine() {
        if (mapManager.GetMachineMap().GetTile(gridPosition) != null) return false;
        
        Vector3Int tempTilePos = gridPosition;
        for (int x = 0; x < mapManager.GetSelectedMultiTile().size.x; x++) {
            for (int y = 0; y < mapManager.GetSelectedMultiTile().size.y; y++) {
                tempTilePos.x = gridPosition.x + x;
                tempTilePos.y = gridPosition.y + y;
                if (mapManager.GetMachineMap().GetTile(tempTilePos) != null) return false;  // Overlapping with another machine part
                if (mapManager.GetConveyorMap().GetTile(tempTilePos) != null) return false; // Overlapping with a conveyor
            }
        }
        return true; // Nothing overlapping
    }

    // Sets proper transparency for any given color
    private Color GetNewTileMapColor(Color colorToAdd) {
        Color newColor = colorToAdd;
        newColor.a = 0.5f;
        return newColor;
    }
    
    // Gets current mouse position
    public Vector3Int GetMousePosition() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mapManager.GetGameMap().WorldToCell(mouseWorldPos);
    }
    
    // Displays multi tile machines
    public void SetMultiHoverDisplay(MultiTile selectedTile) {
        if (selectedTile == null)
            SetRuleHoverDisplay(null);
        else {
            hoverTile = selectedTile.directedTile;
            interactiveMap.ClearAllTiles();
            
            SetTilesMachineHover();
        }
    }

    // Draws tile on map method
    private void SetTilesMachineHover() {
        int counter = 0;
        for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
            for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                Tile newTile = (Tile) ScriptableObject.CreateInstance(typeof(Tile));
                newTile.sprite = mapManager.GetSelectedMultiTile().directedTile[counter].m_AnimatedSprites[spriteEditorManager.GetSelectedTile()];
                interactiveMap.SetTile(gridPosition + new Vector3Int(j, i, 0), newTile); // Draw on map
                counter++;
            }
        }
        ResetTileMapColor(); // Check if overlap happens
    }

    // Rule tile hover display, base method
    public void SetRuleHoverDisplay(RuleTile selectedTile) {
        if (selectedTile == null) {
            hoverTile = MakeNewTile(defaultTile);
            interactiveMap.SetTile(gridPosition, hoverTile[0]);
            interactiveMap.color = GetNewTileMapColor(Color.green);
        }
        else
            hoverTile = MakeNewTile(selectedTile.m_DefaultSprite);
    }

    public void SetSeedHoverDisplay(SeedTile selectedTile) {
        if (selectedTile == null) {
            hoverTile = MakeNewTile(defaultTile);
            interactiveMap.SetTile(gridPosition, hoverTile[0]);
            interactiveMap.color = GetNewTileMapColor(Color.green);
        }
        else
        {
            hoverTile = MakeNewTile(selectedTile.sprite);
        }
          //  
    }

    // Set animation tile hover display override
    public void SetAnimHoverDisplay(AnimatedTile selectedTile) {
        if (selectedTile == null)
            SetRuleHoverDisplay(null);
        else
            hoverTile = MakeNewTile(selectedTile.m_AnimatedSprites[0]);
        interactiveMap.SetTile(GetMousePosition(), selectedTile);
        
    }

    private Tile MakeTile(Sprite newSprite) {
        Tile newTile = (Tile) ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = newSprite;
        return newTile;
    }

    // Makes a new Animated tile
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
