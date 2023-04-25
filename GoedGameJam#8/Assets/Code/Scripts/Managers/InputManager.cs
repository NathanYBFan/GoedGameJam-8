using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
public class InputManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private SpriteEditorManager spriteEditorManager;
    [SerializeField] private bool mouseOverUI;
    private int UILayer;
    [SerializeField, ReadOnly] private bool gameIsPaused;
    [SerializeField] private string pauseMenuSceneName;

    private Vector3Int lastGridPosition, gridPosition;

    private void Start() {
        UILayer = LayerMask.NameToLayer("UI");
        lastGridPosition = new Vector3Int(0, 0, 0);
    }

    void Update()
    {
        // Get mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Get grid that mouse is hovering over
        gridPosition = mapManager.GetGameMap().WorldToCell(mousePosition);
        

        // bool if pointer is over UI element
        mouseOverUI = IsPointerOverUIElement(GetEventSystemRaycastResults());

        if (Input.GetMouseButton(0) && !mouseOverUI)        // Left click, place current selected tile 
            spriteEditorManager.PlaceTileDown(lastGridPosition, gridPosition);
        
        else if (Input.GetMouseButtonDown(1))               // Right click, remove current selected tile
            OnRightClick();

        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)   // Mouse scroll wheel Upwards (away from person)
            spriteEditorManager.RotateSprite(true);
        
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)   // Mouse scroll wheel Downwards (towards person)
            spriteEditorManager.RotateSprite(false);
       
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        
        // Update Grid Positions 
        lastGridPosition = gridPosition;
    }

    private void OnRightClick() {
        mapManager.SetSelectedRuleTile(null);
        mapManager.SetSelectedAnimatedTile(null);
    }

    // Check if pointer is over UI element
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults) {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++) {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
    // Event data
    static List<RaycastResult> GetEventSystemRaycastResults() {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public void PauseGame() 
    {
        if (gameIsPaused) {
            Time.timeScale = 1f;
            SceneManager.UnloadSceneAsync(pauseMenuSceneName);
            gameIsPaused = false;
            return;
        }
        Time.timeScale = 0f;
        SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
        gameIsPaused = true;
    }
}
