using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Initializations")]
    [SerializeField, Required] private AudioMixer audioMixer;
    [SerializeField, Required] private Slider masterVolumeSlider;
    [SerializeField, Required] private Slider musicVolumeSlider;
    [SerializeField, Required] private Slider playerVolumeSlider;
    [SerializeField, Required] private Slider systemVolumeSlider;

    private void Awake()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        playerVolumeSlider.value = PlayerPrefs.GetFloat("playerVolume", 1);
        systemVolumeSlider.value = PlayerPrefs.GetFloat("systemVolume", 1);
    }

    public void MasterVolumeChanged()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolumeSlider.value);
        if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("masterVolume")) * 20f))
            audioMixer.SetFloat("Master", -80f);
        else
            audioMixer.SetFloat("Master", Mathf.Log(masterVolumeSlider.value) * 20f);
    }
    
    public void MusicVolumeChanged()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("musicVolume")) * 20f))
            audioMixer.SetFloat("Music", -80f);
        else
            audioMixer.SetFloat("Music", Mathf.Log(musicVolumeSlider.value) * 20f);
    }
    
    public void PlayerVolumeChanged()
    {
        PlayerPrefs.SetFloat("playerVolume", playerVolumeSlider.value);
        if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("playerVolume")) * 20f))
            audioMixer.SetFloat("Player", -80f);
        else
            audioMixer.SetFloat("Player", Mathf.Log(playerVolumeSlider.value) * 20f);
    }
    
    public void SystemVolumeChanged()
    {
        PlayerPrefs.SetFloat("systemVolume", systemVolumeSlider.value);
        if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("systemVolume")) * 20f))
            audioMixer.SetFloat("System", -80f);
        else
            audioMixer.SetFloat("System", Mathf.Log(systemVolumeSlider.value) * 20f);
    }
}
