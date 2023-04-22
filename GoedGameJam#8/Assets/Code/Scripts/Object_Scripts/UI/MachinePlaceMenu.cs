using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MachinePlaceMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI menuControlButton;
    private Vector3 openedPos;
    private bool menuIsOpened = true;
    [SerializeField] private float menuClosed; 

    // Start is called before the first frame update
    void Start()
    {
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
}