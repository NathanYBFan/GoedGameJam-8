using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;
public class HoverHighlight : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField, ReadOnly] private Tile hoverTile = null;
    [SerializeField] private Sprite defaultTile = null;
    [SerializeField] private Camera cam;
    private Vector3Int previousMousePos = new Vector3Int();

    void Update() {
        Vector3Int mousePos = GetMousePosition();
        Vector3Int gridPosition = mapManager.GetGameMap().WorldToCell(mousePos);

        if (!mousePos.Equals(previousMousePos)) {
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;

            if (mapManager.GetSelectedAnimatedTile() != null && mapManager.GetMachineMap().GetTile(gridPosition) != null && mapManager.GetConveyorMap().GetTile(gridPosition) != null)
                interactiveMap.color = Color.red;
            else
                interactiveMap.color = Color.white;

        }
    }

    private Vector3Int GetMousePosition() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mapManager.GetGameMap().WorldToCell(mouseWorldPos);
    }
    
    public void SetRuleHoverDisplay(RuleTile selectedTile) {
        if (selectedTile == null)
            hoverTile = MakeNewTile(defaultTile);
        else
            hoverTile = MakeNewTile(selectedTile.m_DefaultSprite);
        interactiveMap.SetTile(GetMousePosition(), hoverTile);
    }
    public void SetAnimHoverDisplay(AnimatedTile selectedTile) {
        if (selectedTile == null)
            SetRuleHoverDisplay(null);
        else
            hoverTile = MakeNewTile(selectedTile.m_AnimatedSprites[0]);
        interactiveMap.SetTile(GetMousePosition(), hoverTile);
    }

    private Tile MakeNewTile(Sprite newSprite) {
        Tile newTile = (Tile) ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = newSprite;
        return newTile;
    }
}
