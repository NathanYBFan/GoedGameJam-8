using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteEditorManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private AnimatedTile[] conveyorTiles;
    private int selectedTile = 0;

    public void PlaceTileDown() {
        // Get mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Get grid that mouse is hovering over
        Vector3Int gridPosition = mapManager.GetGameMap().WorldToCell(mousePosition);
        
        // Get clicked tile
        TileBase clickedTile = mapManager.GetGameMap().GetTile(gridPosition);        

        // If a machine is selected
        if (mapManager.GetSelectedRuleTile() == null && mapManager.GetSelectedAnimatedTile() != null) {
            // If tile already has machine on it, then exit
            if (mapManager.GetMachineMap().GetTile(gridPosition) != null || mapManager.GetConveyorMap().GetTile(gridPosition) != null) return;
            // If a conveyor is selected
            if (mapManager.GetSelectedAnimatedTile().name.Contains("Conveyor"))
                mapManager.GetConveyorMap().SetTile(gridPosition, mapManager.GetSelectedAnimatedTile());
            // If any other machine is selected
            else
                mapManager.GetMachineMap().SetTile(gridPosition, mapManager.GetSelectedAnimatedTile());
        }
        // If an environemnt tile is selected
        else if (mapManager.GetSelectedRuleTile() != null && mapManager.GetSelectedAnimatedTile() == null) {
            mapManager.GetGameMap().SetTile(gridPosition, mapManager.GetSelectedRuleTile());
        }
        // If no valid selected tile exit
        else {
            mapManager.GetMachineMap().SetTile(gridPosition, null);
        }
    }

    public void RotateSprite(bool scrollUp) {
        // If selection is null or incorrect type selected, then exit
        if (mapManager.GetSelectedAnimatedTile() == null) return;

        // If scroll was upwards/clockwise
        if (scrollUp) {
            selectedTile++;
            // 0 = Up, 1 = Right, 2 = Down, 3 = Left
            if (selectedTile >= 4) // If selectedTile = 4 (Invalid)
                selectedTile = 0; // Reset to Up
        }
        // If scroll was down/counterclockwise
        else {
            selectedTile--;
            if (selectedTile <= -1)
                selectedTile = 3;
        }
        // Select that tile
        mapManager.SetSelectedAnimatedTile(conveyorTiles[selectedTile]);
    }
}
