using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteEditorManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;    
    [SerializeField] private MachineManager machineManager;     
    [SerializeField] private Grid grid;
    [SerializeField] private AnimatedTile[] conveyorCardinalDirTiles;
    [SerializeField] private AnimatedTile[] conveyorEdgeDirTiles;    
    [SerializeField] private MovementTile[] spawnerTiles;
    [SerializeField] private AudioClip ConveyorAudioClip;
    [SerializeField] private AudioSource audioSource;
    public int selectedTile = 0; //Rotational value

    private bool CheckIfPlaceable(Vector3Int currentGridPos) {
        for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
            for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                if (mapManager.GetMachineMap().GetTile(currentGridPos + new Vector3Int(j, i, 0)) != null) return false;
                if (mapManager.GetConveyorMap().GetTile(currentGridPos + new Vector3Int(j, i, 0)) != null) return false;
            }
        }
        return true;
    }
    public void PlaceTileDown(Vector3Int lastGridPos, Vector3Int currentGridPos) {
        // If a machine is selected
        
        if (mapManager.GetSelectedMultiTile() != null)
        {
            if (mapManager.GetSelectedMultiTile().name == "Extractor")
            {                
                if (!CheckIfPlaceable(currentGridPos)) return;
                Vector3Int offset = Vector3Int.zero;
                int counter = 0;
                for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
                    for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                        if (selectedTile == 0) {
                            offset = currentGridPos + new Vector3Int(1, 1, 0);
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        else if (selectedTile == 1) {
                            offset = currentGridPos + new Vector3Int(1, 0, 0);
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        else if (selectedTile == 2) {
                            offset = currentGridPos;   
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        else if (selectedTile == 3) {
                            offset = currentGridPos + new Vector3Int(0, 1, 0);
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        counter++;
                    }
                }
                if (mapManager.GetGameMap().GetTile(offset).name.Contains("Barren"))
                    SetSpawnTileType(0, offset);
                else if (mapManager.GetGameMap().GetTile(offset).name.Contains("Soil"))
                    SetSpawnTileType(1, offset);
                else if (mapManager.GetGameMap().GetTile(offset).name.Contains("Water"))
                    SetSpawnTileType(2, offset);
                //GameObject spawnLoc = Instantiate(mapManager.GetSelectedMultiTile().spawnLocation, offset + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            }
            else if (mapManager.GetSelectedMultiTile().name == "Combiner")
            {                
                if (!CheckIfPlaceable(currentGridPos)) return;
                Vector3Int offset = Vector3Int.zero;
                int counter = 0;                
                offset = currentGridPos + new Vector3Int(1, 1, 0);
                for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
                    for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                        if (selectedTile == 0) {                            
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        else if (selectedTile == 1) {
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        else if (selectedTile == 2) {
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        else if (selectedTile == 3) {
                            SetupTileTopPlace(offset, counter, currentGridPos, j, i);
                        }
                        counter++;
                    }
                }
                SetupCombinerTiles(offset);
                //GameObject spawnLoc = Instantiate(mapManager.GetSelectedMultiTile().spawnLocation, offset + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            }
        }
        else if (mapManager.GetSelectedAnimatedTile() != null) {
            // If tile already has machine on it, then exit
            if (mapManager.GetMachineMap().GetTile(currentGridPos) != null) return;

            // If a conveyor is selected
            if (mapManager.GetSelectedAnimatedTile().name.Contains("Conveyor")) {
                // Get direction the last grid was
                ConveyorOrientation(lastGridPos - currentGridPos);

                // Fix bends
                FixPreviousConveyor(lastGridPos, currentGridPos);

                mapManager.GetConveyorMap().SetTile(currentGridPos, mapManager.GetSelectedAnimatedTile());
                audioSource.clip = ConveyorAudioClip;
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
        }
        // If an environemnt tile is selected
        else if (mapManager.GetSelectedRuleTile() != null && mapManager.GetSelectedAnimatedTile() == null)
            mapManager.GetGameMap().SetTile(currentGridPos, mapManager.GetSelectedRuleTile());
        
        // If no valid selected tile, then remove
        else {
            if (mapManager.GetMachineMap().GetTile(currentGridPos) != null) {
                switch(mapManager.GetMachineMap().GetTile(currentGridPos).name) {
                    case "Extractor_2":                                         // Middle left
                        RemoveAreaTiles(currentGridPos);
                        break;
                    case "Extractor_3":                                         // Middle right
                        Vector3Int temp = currentGridPos;
                        temp.x -= 1;
                        RemoveAreaTiles(temp);
                        break;
                    case "Extractor_0":                                         // Bottom left
                        temp = currentGridPos; 
                        temp.y += 1;
                        RemoveAreaTiles(temp);
                        break;
                    case "Extractor_1":                                         // Bottom right
                        temp = currentGridPos;
                        temp.x -= 1;
                        temp.y += 1;
                        RemoveAreaTiles(temp);
                        break;
                }
            }
            mapManager.GetConveyorMap().SetTile(currentGridPos, null);
            mapManager.GetHoverHighlight().SetRuleHoverDisplay(null);
        }
    }

    private void SetupCombinerTiles(Vector3Int offset)
    {                               
        CombinerCPUTile combinerCPU = (CombinerCPUTile) ScriptableObject.CreateInstance(typeof(CombinerCPUTile));
        ContainerTile container1 = (ContainerTile) ScriptableObject.CreateInstance(typeof(ContainerTile));
        ContainerTile container2 = (ContainerTile) ScriptableObject.CreateInstance(typeof(ContainerTile));        
        OutputTile output = (OutputTile) ScriptableObject.CreateInstance(typeof(OutputTile));
        combinerCPU.name = "Extractor_4";
        combinerCPU.sprite = mapManager.GetSelectedMultiTile().directedTile[4].m_AnimatedSprites[selectedTile];          
        combinerCPU.input1 = container1;
        combinerCPU.input2 = container2;        
        combinerCPU.output = output;  
        combinerCPU.recipes = machineManager.recipes;     
        switch(selectedTile)
        {
            case 1:
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, -1, 0), container2);                              
                output.movementDir = new Vector2(0.1f, 0);                                              
                output.position = offset + new Vector3Int(-1, 0, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 0, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;
            case 2:
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 1, 0), container2);                              
                output.movementDir = new Vector2(0, -0.1f);                                   
                output.position = offset + new Vector3Int(0, -1, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, -1, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;
            case 3:
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, -1, 0), container2);                              
                output.movementDir = new Vector2(-0.1f, 0);                                   
                output.position = offset + new Vector3Int(1, 0, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 0, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;  
            default:
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, -1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, -1, 0), container2);                              
                output.movementDir = new Vector2(0, 0.1f);                                     
                output.position = offset + new Vector3Int(0, 1, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, 1, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;
        }            

    }
    private void SetupTileTopPlace(Vector3Int offset, int counter, Vector3Int currentGridPos, int j, int i) {
        mapManager.GetSelectedMultiTile().localOutputLocation = offset;                        
        Tile newTile = (Tile) ScriptableObject.CreateInstance(typeof(Tile));
        newTile.name = "Extractor_" + counter;
        newTile.sprite = mapManager.GetSelectedMultiTile().directedTile[counter].m_AnimatedSprites[selectedTile];                   
        mapManager.GetMachineMap().SetTile(currentGridPos + new Vector3Int(j, i, 0), newTile);
    }

    private void SetSpawnTileType(int temp, Vector3Int offset)
    {
        switch(selectedTile)
        {
            case 1:
                spawnerTiles[temp].movementDir = new Vector2(0.1f, 0);
                break;
            case 2:
                spawnerTiles[temp].movementDir = new Vector2(0, -0.1f);
                break;
            case 3:
                spawnerTiles[temp].movementDir = new Vector2(-0.1f, 0);
                break;  
            default:
                spawnerTiles[temp].movementDir = new Vector2(0, 0.1f);
                break;
        }                
        mapManager.GetMachineMap().SetTile(offset, spawnerTiles[temp]);
    }
    private void RemoveAreaTiles(Vector3Int startPos) {
        Vector3Int gridPositions = startPos;
        for (int x = 0; x < 2; x++) {
            for (int y = 0; y < 2; y++) {
                gridPositions.x = startPos.x + x;
                gridPositions.y = startPos.y - y;
                mapManager.GetMachineMap().SetTile(gridPositions, null);
            }
        }
    }

    public void UpdateSpawnerTiles()
    {
        Vector3Int gridBottomLeft = grid.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)));
        Vector3Int gridTopRight = grid.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)));
        machineManager.dirtSpawners = new();
        machineManager.soilSpawners = new();
        machineManager.waterSpawners = new();        
        machineManager.combiners = new();
        //First iteration of terrain gen
        for (int y = gridBottomLeft.y; y <= gridTopRight.y; y++)
        {
            for (int x = gridBottomLeft.x; x <= gridTopRight.x; x++)
            {
                if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)) == null) continue;
                if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)).name.Contains("Dirt"))
                {
                    machineManager.dirtSpawners.Add(new Vector3(x + 0.5f, y + 0.5f, 0));
                }
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)).name.Contains("Soil"))
                {                    
                    machineManager.soilSpawners.Add(new Vector3(x + 0.5f, y + 0.5f, 0));
                }                
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)).name.Contains("Water"))
                {                    
                    machineManager.waterSpawners.Add(new Vector3(x + 0.5f, y + 0.5f, 0));
                }
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)) is CombinerCPUTile combiner)
                {
                    machineManager.combiners.Add(combiner);
                }
            }
        }
        
        //Get machine map
        //Check which tiles have spawner in its name. If has it, add to list of spawner tiles (machineManager)
    }

    public void RotateSprite(bool scrollUp) { // Order is down, left, up, right
        // If selection is null or incorrect type selected, then exit
        if (mapManager.GetSelectedRuleTile() != null) return;
        
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
        if (mapManager.GetSelectedAnimatedTile() != null)
            SelectCardinalSprite(selectedTile);
        else if (mapManager.GetSelectedMultiTile() != null) {
            mapManager.SetSelectedMultiTile(mapManager.GetSelectedMultiTile());
        }
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

    public int GetSelectedTile() { return selectedTile; }
}
