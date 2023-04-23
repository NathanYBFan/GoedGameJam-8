using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Tilemap gameMap;
    [SerializeField] private MapManager mapManager;

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = gameMap.WorldToCell(mousePosition);
            
            TileBase clickedTile = gameMap.GetTile(gridPosition);
            if (mapManager.GetSelectedTile() == null)
                return;
            else
                gameMap.SetTile(gridPosition, mapManager.GetSelectedTile());
        }

        if (Input.GetMouseButtonDown(1)) { mapManager.SetSelectedTile(null); }
    }
}
