using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;

public class SpriteEditorManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;    
    [SerializeField] private MachineManager machineManager;    
    [SerializeField] private InventoryManager resourceManager;  
    [SerializeField] private Grid grid;
    [SerializeField] private AnimatedTile[] conveyorCardinalDirTiles;
    [SerializeField] private AnimatedTile[] conveyorEdgeDirTiles;    
    [SerializeField] private MovementTile[] spawnerTiles;
    [SerializeField] private AudioClip ConveyorAudioClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField, ReadOnly] private int selectedTile = 0; // Rotational value

    // Main controller method
    public void PlaceTileDown(Vector3Int lastGridPos, Vector3Int currentGridPos) {
        
        // If a machine is selected
        if (mapManager.GetSelectedMultiTile() != null) {
            if (!CheckIfPlaceable(currentGridPos)) return;

            if (mapManager.GetSelectedMultiTile().name == "Extractor")
                PlaceExtractor(currentGridPos);
            else if (mapManager.GetSelectedMultiTile().name == "Combiner")
                PlaceCombiner(currentGridPos);
            else if (mapManager.GetSelectedMultiTile().name == "Breeder")
                PlaceBreeder(currentGridPos);
            else if (mapManager.GetSelectedMultiTile().name == "Incinerator")
                PlaceIncinerator(currentGridPos);
            else if (mapManager.GetSelectedMultiTile().name == "Uploader")                
                PlaceUploader(currentGridPos);
        }

        // If Conveyor Belts are selected
        else if (mapManager.GetSelectedAnimatedTile() != null)
            PlaceConveyorBelt(lastGridPos, currentGridPos);
        
        // If an environemnt tile is selected
        else if (mapManager.GetSelectedRuleTile() != null && mapManager.GetSelectedAnimatedTile() == null) {
            if (mapManager.GetGameMap().GetTile(currentGridPos) == mapManager.GetSelectedRuleTile()) return;
            else mapManager.GetGameMap().SetTile(currentGridPos, mapManager.GetSelectedRuleTile());
        }
        
        // If no valid selected tile, then remove
        else
            RemoveTiles(currentGridPos);
    }

    // Places the entire extractor
    private void PlaceExtractor(Vector3Int currentGridPos) {
        int counter = 0;
        Vector3Int offset = Vector3Int.zero;
        for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
            for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                if (selectedTile == 0) {
                    offset = currentGridPos + new Vector3Int(1, 1, 0);
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                else if (selectedTile == 1) {
                    offset = currentGridPos + new Vector3Int(1, 0, 0);
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                else if (selectedTile == 2) {
                    offset = currentGridPos + new Vector3Int(0, 0, 0);   
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                else if (selectedTile == 3) {
                    offset = currentGridPos + new Vector3Int(0, 1, 0);
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                counter++;
            }
        }

        // Set a spawn tile
        if (mapManager.GetGameMap().GetTile(offset).name.Contains("Barren"))
            SetSpawnTileType(0, offset);
        else if (mapManager.GetGameMap().GetTile(offset).name.Contains("Soil"))
            SetSpawnTileType(1, offset);
        else if (mapManager.GetGameMap().GetTile(offset).name.Contains("Water"))
            SetSpawnTileType(2, offset);
        else if (mapManager.GetGameMap().GetTile(offset).name.Contains("Grass"))
            SetSpawnTileType(3, offset);
        else if (mapManager.GetGameMap().GetTile(offset).name.Contains("Seed"))
            SetSpawnTileType(4, offset);
    }

    private void PlaceCombiner(Vector3Int currentGridPos) {
        int counter = 0;
        Vector3Int offset = Vector3Int.zero;
        offset = currentGridPos + new Vector3Int(1, 1, 0);
        for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
            for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Combiner_");
                counter++;
            }
        }
        SetupCombinerTiles(offset);
    }

    private void PlaceBreeder(Vector3Int currentGridPos) {
        int counter = 0;
        Vector3Int offset = Vector3Int.zero;
        for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
            for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                if (selectedTile == 0) {             
                    offset = currentGridPos + new Vector3Int(0, 1, 0);              
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                else if (selectedTile == 1) {                            
                    offset = currentGridPos + new Vector3Int(0, 0, 0);         
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                else if (selectedTile == 2) {
                    offset = currentGridPos + new Vector3Int(1, 0, 0);
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                else if (selectedTile == 3) {
                    offset = currentGridPos + new Vector3Int(1, 1, 0);
                    SetupTileTopPlace(offset, counter, currentGridPos, j, i, "Extractor_");
                }
                counter++;
            }
        }
        SetupBreederTiles(offset);
    }

    private void PlaceIncinerator(Vector3Int currentGridPos) {
        Vector3Int offset = Vector3Int.zero;
        offset = currentGridPos;

        IncineratorTile incinerator = (IncineratorTile) ScriptableObject.CreateInstance(typeof(IncineratorTile));
        incinerator.name = "Incinerator";
        incinerator.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];        
        incinerator.position = offset + new Vector3(0.5f, 0.5f, 0); 
        incinerator.recipes = machineManager.incineratingRecipes;
        switch (selectedTile)
        {
            case 1:
                incinerator.movementDir = new Vector2(0.1f, 0);
                break;
            case 2:
                incinerator.movementDir = new Vector2(0, -0.1f);
                break;
            case 3:
                incinerator.movementDir = new Vector2(-0.1f, 0);
                break;
            default:
                incinerator.movementDir = new Vector2(0, 0.1f);
                break;
        }
        mapManager.GetMachineMap().SetTile(offset, incinerator);
    }

    private void PlaceUploader(Vector3Int currentGridPos) {
        Vector3Int offset = Vector3Int.zero;
        offset = currentGridPos;

        UploaderTile uploader = (UploaderTile) ScriptableObject.CreateInstance(typeof(UploaderTile));
        uploader.name = "Incinerator";  // For removing the tile, saved as remove 1 tile
        uploader.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];      
        uploader.resourceManager = resourceManager;        
        mapManager.GetMachineMap().SetTile(offset, uploader);
    }

    private void PlaceConveyorBelt(Vector3Int lastGridPos, Vector3Int currentGridPos) {
        // If tile already has machine on it, then exit
        if (mapManager.GetMachineMap().GetTile(currentGridPos) != null) return;
        // If a conveyor is not selected, then exit
        if (!mapManager.GetSelectedAnimatedTile().name.Contains("Conveyor")) return;

        // Get direction the last grid was
        ConveyorOrientation(lastGridPos - currentGridPos);
        // Fix bends
        FixPreviousConveyor(lastGridPos, currentGridPos);

        // Set current position tile
        mapManager.GetConveyorMap().SetTile(currentGridPos, mapManager.GetSelectedAnimatedTile());
        // Play Audio
        audioSource.clip = ConveyorAudioClip;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    private void RemoveTiles(Vector3Int currentGridPos) {
        mapManager.GetConveyorMap().SetTile(currentGridPos, null);
        mapManager.GetHoverHighlight().SetRuleHoverDisplay(null);

        // If theres nothing to remove, then exit
        if (mapManager.GetMachineMap().GetTile(currentGridPos) == null) return;
        
        // Removal based on name
        Vector3Int temp = currentGridPos;
        switch(mapManager.GetMachineMap().GetTile(currentGridPos).name) {
            case "Extractor_2":                                         // Top left
                RemoveAreaTiles(currentGridPos, 2, 2);
                break;
            case "Extractor_3":                                         // Top right        
                temp.x -= 1;
                RemoveAreaTiles(temp, 2, 2);
                break;
            case "Extractor_0":                                         // Bottom left
                temp.y += 1;
                RemoveAreaTiles(temp, 2, 2);
                break;
            case "Extractor_1":                                         // Bottom right
                temp.x -= 1;
                temp.y += 1;
                RemoveAreaTiles(temp, 2, 2);
                break;
            case "Incinerator":
                mapManager.GetMachineMap().SetTile(currentGridPos, null);
                break;
            case "Combiner_0":                                          // Top left
                temp.y += 2;
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_1":                                          // Top middle
                temp.y += 2;
                temp.x -= 1;
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_2":                                          // Top right
                temp.y += 2;
                temp.x -= 2;
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_3":                                          // Middle left
                temp.y += 1;
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_4":                                          // Middle middle
                temp.y += 1;
                temp.x -= 1;
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_5":                                          // Middle right
                temp.y += 1;
                temp.x -= 2;
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_6":                                          // Top left
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_7":                                          // Top middle
                temp.x -= 1;
                RemoveAreaTiles(temp, 3, 3);
                break;
            case "Combiner_8":                                          // Top right
                temp.x -= 2;
                RemoveAreaTiles(temp, 3, 3);
                break;
        }
    }

    private void SetupBreederTiles(Vector3Int offset)
    {                               
        BreederCPUTile breederCPU = (BreederCPUTile) ScriptableObject.CreateInstance(typeof(BreederCPUTile));
        ContainerTile container1 = (ContainerTile) ScriptableObject.CreateInstance(typeof(ContainerTile));
        ContainerTile container2 = (ContainerTile) ScriptableObject.CreateInstance(typeof(ContainerTile));        
        OutputTile output = (OutputTile) ScriptableObject.CreateInstance(typeof(OutputTile));
        breederCPU.input1 = container1;
        breederCPU.input2 = container2;        
        breederCPU.output = output;  
        breederCPU.animalRecipes = machineManager.animalRecipes;     
        switch(selectedTile)
        {
            case 1: // Facing right
                breederCPU.name = "Extractor_0";
                container1.name = "Extractor_1";
                container2.name = "Extractor_3";
                output.name = "Extractor_2";

                breederCPU.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];        
                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[1].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[3].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[2].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 0, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 1, 0), container2);                              
                output.movementDir = new Vector2(0.1f, 0);                                              
                output.position = offset + new Vector3Int(0, 1, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, 1, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, breederCPU);
                break;
            case 2: // Facing up
                breederCPU.name = "Extractor_1";
                container1.name = "Extractor_3";
                container2.name = "Extractor_2";
                output.name = "Extractor_0";

                breederCPU.sprite = mapManager.GetSelectedMultiTile().directedTile[1].m_AnimatedSprites[selectedTile];          
                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[3].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[2].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, 1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 1, 0), container2);                              
                output.movementDir = new Vector2(0, -0.1f);                                   
                output.position = offset + new Vector3Int(-1, 0, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 0, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, breederCPU);
                break;
            case 3: // Facing left
                breederCPU.name = "Extractor_3";
                container1.name = "Extractor_2";
                container2.name = "Extractor_0";
                output.name = "Extractor_1";

                breederCPU.sprite = mapManager.GetSelectedMultiTile().directedTile[3].m_AnimatedSprites[selectedTile];          
                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[2].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[1].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 0, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, -1, 0), container2);                              
                output.movementDir = new Vector2(-0.1f, 0);                                   
                output.position = offset + new Vector3Int(0, -1, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, -1, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, breederCPU);
                break;  
            default: // Facing down
                breederCPU.name = "Extractor_2";
                container1.name = "Extractor_0";
                container2.name = "Extractor_1";
                output.name = "Extractor_3";

                breederCPU.sprite = mapManager.GetSelectedMultiTile().directedTile[2].m_AnimatedSprites[selectedTile];          
                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[1].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[3].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, -1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, -1, 0), container2);                              
                output.movementDir = new Vector2(0, 0.1f);                                     
                output.position = offset + new Vector3Int(1, 0, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 0, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, breederCPU);
                break;
        }            

    }
    private void SetupCombinerTiles(Vector3Int offset)
    {                               
        CombinerCPUTile combinerCPU = (CombinerCPUTile) ScriptableObject.CreateInstance(typeof(CombinerCPUTile));
        ContainerTile container1 = (ContainerTile) ScriptableObject.CreateInstance(typeof(ContainerTile));
        ContainerTile container2 = (ContainerTile) ScriptableObject.CreateInstance(typeof(ContainerTile));        
        OutputTile output = (OutputTile) ScriptableObject.CreateInstance(typeof(OutputTile));
        combinerCPU.sprite = mapManager.GetSelectedMultiTile().directedTile[4].m_AnimatedSprites[selectedTile];          
        combinerCPU.input1 = container1;
        combinerCPU.input2 = container2;        
        combinerCPU.output = output;  
        combinerCPU.recipes = machineManager.recipes;     
        switch(selectedTile)
        {
            case 1: // Facing left
                combinerCPU.name = "Combiner_4";
                container1.name = "Combiner_8";
                container2.name = "Combiner_2";
                output.name = "Combiner_3";

                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[8].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[2].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[3].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, -1, 0), container2);                              
                output.movementDir = new Vector2(0.1f, 0);                                              
                output.position = offset + new Vector3Int(-1, 0, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 0, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;
            case 2: // Facing up
                combinerCPU.name = "Combiner_4";
                container1.name = "Combiner_6";
                container2.name = "Combiner_8";
                output.name = "Combiner_1";

                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[6].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[8].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[1].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 1, 0), container2);                              
                output.movementDir = new Vector2(0, -0.1f);                                   
                output.position = offset + new Vector3Int(0, -1, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, -1, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;
            case 3: // Facing right
                combinerCPU.name = "Combiner_4";
                container1.name = "Combiner_6";
                container2.name = "Combiner_0";
                output.name = "Combiner_5";

                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[6].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[5].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, 1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, -1, 0), container2);                              
                output.movementDir = new Vector2(-0.1f, 0);                                   
                output.position = offset + new Vector3Int(1, 0, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, 0, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;  
            default: // Facing down
                combinerCPU.name = "Combiner_4";
                container1.name = "Combiner_0";
                container2.name = "Combiner_2";
                output.name = "Combiner_7";

                container1.sprite = mapManager.GetSelectedMultiTile().directedTile[0].m_AnimatedSprites[selectedTile];                    
                container2.sprite = mapManager.GetSelectedMultiTile().directedTile[2].m_AnimatedSprites[selectedTile];                    
                output.sprite = mapManager.GetSelectedMultiTile().directedTile[7].m_AnimatedSprites[selectedTile];
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(-1, -1, 0), container1);                
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(1, -1, 0), container2);                              
                output.movementDir = new Vector2(0, 0.1f);                                     
                output.position = offset + new Vector3Int(0, 1, 0);  
                mapManager.GetMachineMap().SetTile(offset + new Vector3Int(0, 1, 0), output);                
                mapManager.GetMachineMap().SetTile(offset, combinerCPU);
                break;
        }            

    }
    private void SetupTileTopPlace(Vector3Int offset, int counter, Vector3Int currentGridPos, int j, int i, string machineName) {
        mapManager.GetSelectedMultiTile().localOutputLocation = offset;                        
        Tile newTile = (Tile) ScriptableObject.CreateInstance(typeof(Tile));
        newTile.name = machineName + counter;
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

    // Removes all machine tiles in a rectangular/square grid
    private void RemoveAreaTiles(Vector3Int startPos, int areaX, int areaY) {
        Vector3Int gridPositions = startPos;
        for (int x = 0; x < areaX; x++) {
            for (int y = 0; y < areaY; y++) {
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
        machineManager.grassSpawners = new();
        machineManager.waterSpawners = new();   
        machineManager.seedSpawners = new();     
        machineManager.combiners = new();          
        machineManager.breeders = new();
        machineManager.incinerators = new();        
        machineManager.uploaders = new();
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
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)).name.Contains("Grass"))
                {                    
                    machineManager.grassSpawners.Add(new Vector3(x + 0.5f, y + 0.5f, 0));
                }
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)).name.Contains("Seed"))
                {                                       
                    machineManager.seedSpawners.Add(new Vector3(x + 0.5f, y + 0.5f, 0));
                }
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)) is CombinerCPUTile combiner)
                {
                    machineManager.combiners.Add(combiner);
                }
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)) is BreederCPUTile breeder)
                {
                    machineManager.breeders.Add(breeder);
                }
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)) is IncineratorTile incinerator)
                {
                    machineManager.incinerators.Add(incinerator);
                }
                else if (mapManager.GetMachineMap().GetTile(new Vector3Int(x, y, 0)) is UploaderTile uploader)
                {
                    machineManager.uploaders.Add(uploader);
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
        audioSource.Play();
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
    private bool CheckIfPlaceable(Vector3Int currentGridPos) {
        for (int i = 0; i < mapManager.GetSelectedMultiTile().size.x; i++) {
            for (int j = 0; j < mapManager.GetSelectedMultiTile().size.y; j++) {
                if (mapManager.GetMachineMap().GetTile(currentGridPos + new Vector3Int(j, i, 0)) != null) return false;
                if (mapManager.GetConveyorMap().GetTile(currentGridPos + new Vector3Int(j, i, 0)) != null) return false;
            }
        }
        return true;
    }
    public int GetSelectedTile() { return selectedTile; }
}
