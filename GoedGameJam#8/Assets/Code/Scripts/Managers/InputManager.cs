using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private bool mouseOverUI;
    private int UILayer;

    private void Start() {
        UILayer = LayerMask.NameToLayer("UI");
    }

    void Update()
    {
        if (IsPointerOverUIElement())
            mouseOverUI = true;
        else
            mouseOverUI = false;

        // Left click, place current selected tile 
        if (Input.GetMouseButton(0) && !mouseOverUI) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = mapManager.GetGameMap().WorldToCell(mousePosition);
            
            TileBase clickedTile = mapManager.GetGameMap().GetTile(gridPosition);
            if (mapManager.GetSelectedTile() == null)
                return;
            else {
                switch(mapManager.GetTileType(mapManager.GetSelectedTile())) {
                    case Enums.TileTypes.machines:
                        mapManager.GetMachineMap().SetTile(gridPosition, mapManager.GetSelectedTile());
                        break;
                    case Enums.TileTypes.environment:
                        mapManager.GetGameMap().SetTile(gridPosition, mapManager.GetSelectedTile());
                        break;
                    default:
                        Debug.Log("This item's data does not exist");
                        break;
                }
            }
        }
        // Right click, remove current selected tile
        else if (Input.GetMouseButtonDown(1)) { mapManager.SetSelectedTile(null); }
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
