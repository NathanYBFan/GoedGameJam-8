using UnityEngine;
using UnityEngine.Tilemaps;
public class HoverHighlight : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile defaultTile = null;
    [SerializeField] private Camera cam;
    private Vector3Int previousMousePos = new Vector3Int();

    void Update() {
        Vector3Int mousePos = GetMousePosition();
        
        if (!mousePos.Equals(previousMousePos)) {
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }
    }

    private Vector3Int GetMousePosition() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mapManager.GetGameMap().WorldToCell(mouseWorldPos);
    }
    
    public void SetHoverDisplay(Tile selectedTile) {
        if (selectedTile == null)
            hoverTile = defaultTile;
        else
            hoverTile = selectedTile;
        interactiveMap.SetTile(GetMousePosition(), hoverTile);
    } 
}
