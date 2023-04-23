using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class LeftPopupPlaceMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI menuControlButton;
    [SerializeField] private float menuClosed; 
    [SerializeField] MapManager mapManager;
    private Vector3 openedPos;
    private bool menuIsOpened = true;

    // Start is called before the first frame update
    void Start() {
        openedPos = menu.transform.position;
    }

    public void menuButtonPressed () {
        if (menuIsOpened)
            CloseMenu();
        else
            OpenMenu();
    }

    private void OpenMenu() {
        menuIsOpened = true;
        menu.transform.position = openedPos;
        menuControlButton.text = "<";
    }

    private void CloseMenu() {
        menuIsOpened = false;
        menu.transform.position = new Vector3(openedPos.x - menuClosed, openedPos.y, openedPos.z);
        menuControlButton.text = ">";
    }

    public void SelectItemToPlace(RuleTile building) {
        mapManager.SetSelectedTile(building);
    }
    public void SelectItemToPlace() {
        mapManager.SetSelectedTile(null);
    }
}