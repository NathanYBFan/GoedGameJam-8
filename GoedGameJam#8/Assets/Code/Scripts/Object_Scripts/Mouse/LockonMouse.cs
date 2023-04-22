using UnityEngine;
using UnityEngine.Tilemaps;
public class LockonMouse : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Camera cam;

    private Vector3Int previousMousePos = new Vector3Int();

    void Awake() {
        grid = gameObject.GetComponent<Grid>();
    }

    void Update() {
        Vector3Int mousePos = GetMousePosition();
        
        if (!mousePos.Equals(previousMousePos)) {
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }
    }

    Vector3Int GetMousePosition () {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}
