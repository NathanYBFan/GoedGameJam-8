using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Tilemap gameMap;
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

        if (Input.GetMouseButton(0) && !mouseOverUI) {
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
