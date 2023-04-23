using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class VideoSettings : MonoBehaviour
{
    [FormerlySerializedAs("VSyncCheckmark")]
    [Header("Vsync Initializations")]
    [SerializeField, Required] private GameObject vSyncCheckmark;
    
    private TMP_Dropdown windowedStateDropDown;
    private TMP_Dropdown resolutionDropDown;
    
    private bool vSyncOn;

    private void Awake()
    {
        WindowStateChanged(PlayerPrefs.GetInt("windowState", 0));
        ResolutionChanged(PlayerPrefs.GetInt("resolution", 1));
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("vSync", 0);
        if (PlayerPrefs.GetInt("vSync", 1) == 0) {
            vSyncOn = false;
            vSyncCheckmark.SetActive(vSyncOn);
        }
        else {
            vSyncOn = true;
            vSyncCheckmark.SetActive(vSyncOn);
        }
        windowedStateDropDown = GameObject.Find("WindowedStateDropDown").GetComponent<TMP_Dropdown>();
        windowedStateDropDown.value = PlayerPrefs.GetInt("windowState", 0);
        resolutionDropDown = GameObject.Find("ResolutionDropDown").GetComponent<TMP_Dropdown>();
        resolutionDropDown.value = PlayerPrefs.GetInt("resolution", 1);
    }
    
    public void WindowStateChanged(TMP_Dropdown dropDown)
    {
        WindowStateChanged(dropDown.value);
    }

    private void WindowStateChanged(int value)
    {
        PlayerPrefs.SetInt("windowState", value); // Save the value to the player prefs
        Screen.fullScreenMode = value switch
        {
            0 => // Borderless
                FullScreenMode.FullScreenWindow,
            1 => // Windowed
                FullScreenMode.Windowed,
            2 => // Fullscreen
                FullScreenMode.ExclusiveFullScreen,
            _ => Screen.fullScreenMode
        };
    }

    public void ResolutionChanged(TMP_Dropdown dropdown)
    {
        ResolutionChanged(dropdown.value);
    }

    private void ResolutionChanged(int value)
    {
        // We need to check if the window state is fullscreen, as we need to pass that to the Screen.SetResolution method
        bool fullscreen = PlayerPrefs.GetInt("windowState", 0) == 2 || PlayerPrefs.GetInt("windowState", 0) == 0;
        PlayerPrefs.SetInt("resolution", value);
        switch (value)
        {
            case 0:
                Screen.SetResolution(2560, 1440, fullscreen);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, fullscreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, fullscreen);
                break;
        }
    }

    public void VSyncChanged()
    {
        if (PlayerPrefs.GetInt("vSync", 0) == 1) // Turn VSync off
        {
            QualitySettings.vSyncCount = 0; // VSync turned off
            vSyncCheckmark.SetActive(false);
            PlayerPrefs.SetInt("vSync", 0);
        }
        else // Turn VSync on
        {
            QualitySettings.vSyncCount = 1; // Vsync On
            vSyncCheckmark.SetActive(true);
            PlayerPrefs.SetInt("vSync", 1);
        }
        vSyncOn = !vSyncOn;
    }
}
