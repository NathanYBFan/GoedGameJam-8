using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class LeftPopupPlaceMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI menuControlButton;
    [SerializeField] private Transform menuOpenedPos;
    [SerializeField] private Transform menuClosedPos; 
    [SerializeField] MapManager mapManager;
    private bool menuIsOpened = true;
    public void menuButtonPressed () {
        if (menuIsOpened)
            CloseMenu();
        else
            OpenMenu();
    }

    private void OpenMenu() {
        menuIsOpened = true;
        menu.transform.position = menuOpenedPos.position;
        menuControlButton.text = "<";
    }

    private void CloseMenu() {
        menuIsOpened = false;
        menu.transform.position = menuClosedPos.position;
        menuControlButton.text = ">";
    }

    public void SelectRuleTileToPlace(RuleTile building) {
        mapManager.SetSelectedRuleTile(building);
    }
    public void SelectSeedTileToPlace(SeedTile plant) {
        mapManager.SetSelectedSeedTile(plant);
    }
    public void SelectConveyorToPlace(AnimatedTile conveyor) {
        mapManager.SetSelectedAnimatedTile(conveyor);
    }
    public void SelectMachineToPlace(MultiTile machine) {
        mapManager.SetSelectedMultiTile(machine);

    }
}