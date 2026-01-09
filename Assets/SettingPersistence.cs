using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPersistence : MonoBehaviour
{
    [Header("UI References")]
    public Slider volumeSlider;
    public Toggle vsyncToggle;

    [Header("Audio Reference")]
    public AudioSource musicSource; // This creates the 3rd slot!

    private void Start()
    {
        LoadUserSettings();
        volumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        vsyncToggle.onValueChanged.AddListener(delegate { SaveUserSettings(); });
    }

    public void UpdateVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume = volumeSlider.value;
        }
        SaveUserSettings();
    }

    public void SaveUserSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
        int vsyncValue = vsyncToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSyncState", vsyncValue);
        QualitySettings.vSyncCount = vsyncValue;
        PlayerPrefs.Save();
    }

    public void LoadUserSettings()
    {
        float savedVol = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        volumeSlider.value = savedVol;
        if (musicSource != null) musicSource.volume = savedVol;

        int savedVSync = PlayerPrefs.GetInt("VSyncState", 1);
        vsyncToggle.isOn = (savedVSync == 1);
        QualitySettings.vSyncCount = savedVSync;
    }
}