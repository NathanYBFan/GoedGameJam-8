using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteEditorManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private AnimatedTile[] conveyorCardinalDirTiles;
    [SerializeField] private AnimatedTile[] conveyorEdgeDirTiles;
    
    private int selectedTile = 0;

    public bool CheckIfPlaceable(Vector3Int currentGridPos)
    {
        for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++)
        {
            for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++)
            {
                if(mapManager.GetMachineMap().GetTile(currentGridPos + new Vector3Int(j, i, 0)) != null) return false;
            }
        }
        return true;
    }
    public void PlaceTileDown(Vector3Int lastGridPos, Vector3Int currentGridPos) {
        // If a machine is selected
        
        if (mapManager.GetSelectedMultiTile() != null)
        {
            if (!CheckIfPlaceable(currentGridPos)) return;

            int counter = 0;
            for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++)
            {
                for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++)
                {
                    
                    Debug.Log("Here");
                    mapManager.GetMachineMap().SetTile(currentGridPos + new Vector3Int(j, i, 0), mapManager.GetSelectedMultiTile().multiTile[counter]);
                    counter++;
                }
            }
        }
        else if (mapManager.GetSelectedRuleTile() == null && mapManager.GetSelectedAnimatedTile() != null) {
            // If tile already has machine on it, then exit
            if (mapManager.GetMachineMap().GetTile(currentGridPos) != null) return;

            // If a conveyor is selected
            if (mapManager.GetSelectedAnimatedTile().name.Contains("Conveyor")) {
                // Get direction the last grid was
                ConveyorOrientation(lastGridPos - currentGridPos);

                // Fix bends
                FixPreviousConveyor(lastGridPos, currentGridPos);

                mapManager.GetConveyorMap().SetTile(currentGridPos, mapManager.GetSelectedAnimatedTile());
            }
        }
        // If an environemnt tile is selected
        else if (mapManager.GetSelectedRuleTile() != null && mapManager.GetSelectedAnimatedTile() == null) {
            mapManager.GetGameMap().SetTile(currentGridPos, mapManager.GetSelectedRuleTile());
        }
        // If no valid selected tile, then remove
        else {
           // mapManager.GetMachineMap().SetTile(currentGridPos, null);
            mapManager.GetConveyorMap().SetTile(currentGridPos, null);
        }
    }

    public void RotateSprite(bool scrollUp) { // Order is down, left, up, right
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
        SelectCardinalSprite(selectedTile);
    }

    private void SelectCardinalSprite(int spriteToPick) {
        mapManager.SetSelectedAnimatedTile(conveyorCardinalDirTiles[spriteToPick]);
    }
    
    private void SetDiagonalSprite(Vector3Int position, int spriteToPick) {
        mapManager.GetConveyorMap().SetTile(position, conveyorEdgeDirTiles[spriteToPick]);
    }

    private void SetOppositeSprite(Vector3Int position, int spriteToPick) {
        mapManager.GetConveyorMap().SetTile(position, conveyorCardinalDirTiles[spriteToPick]);
    }

    private void FixPreviousConveyor(Vector3Int lastGridPos, Vector3Int currentGridPos) {
        if (mapManager.GetConveyorMap().GetTile(lastGridPos) == null) return; // Nothing happens
        if (mapManager.GetConveyorMap().GetTile(lastGridPos) == mapManager.GetSelectedAnimatedTile()) return;

        // Up and to the sides
        if (mapManager.GetConveyorMap().GetTile(lastGridPos).name.Contains("Conveyor_Up")) {
            if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Left")
                SetDiagonalSprite(lastGridPos, 0);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Right")
                SetDiagonalSprite(lastGridPos, 1);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Down")
                SetOppositeSprite(lastGridPos, 2);
        }
        // Down and to the sides
        else if (mapManager.GetConveyorMap().GetTile(lastGridPos).name.Contains("Conveyor_Down")) {
            if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Left")
                SetDiagonalSprite(lastGridPos, 6);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Right")
                SetDiagonalSprite(lastGridPos, 7);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Up")
                SetOppositeSprite(lastGridPos, 0);
        }
        // Left and UP/DOWN
        else if (mapManager.GetConveyorMap().GetTile(lastGridPos).name.Contains("Conveyor_Left")) {
            if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Up")
                SetDiagonalSprite(lastGridPos, 5);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Down")
                SetDiagonalSprite(lastGridPos, 4);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Right")
                SetOppositeSprite(lastGridPos, 1);
        }
        // Right and UP/DOWN
        else if (mapManager.GetConveyorMap().GetTile(lastGridPos).name.Contains("Conveyor_Right")) {
            if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Up")
                SetDiagonalSprite(lastGridPos, 3);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Down")
                SetDiagonalSprite(lastGridPos, 2);
            else if (mapManager.GetSelectedAnimatedTile().name == "Conveyor_Left")
                SetOppositeSprite(lastGridPos, 3);
        }
    }

    private void ConveyorOrientation(Vector3Int direction) {
        switch (direction) {
            case Vector3Int v when v.Equals(Vector3Int.up): // Moving mouse down
                SelectCardinalSprite(2); // Down
                break;
            case Vector3Int v when v.Equals(Vector3Int.down): // Moving mouse up
                SelectCardinalSprite(0); // Up
                break;
            case Vector3Int v when v.Equals(Vector3Int.left): // Moving mouse right
                SelectCardinalSprite(1); // Right
                break;
            case Vector3Int v when v.Equals(Vector3Int.right): // Moving mouse left
                SelectCardinalSprite(3); // Left
                break;
            default:
                break;
        }
    }
}
