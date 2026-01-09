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
    public static SettingsPersistence Instance; // Shared global reference

    private void Awake()
    {
        // If an instance already exists and it's not THIS one...
        if (Instance != null && Instance != this)
        {
            Debug.Log("Duplicate SoundManager found in scene, destroying it.");
            Destroy(gameObject); // Kill the new one immediately
            return; // Stop the rest of the Awake code from running
        }

        // Otherwise, this is the first one, so make it persistent
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    private void Start()
    {
        LoadUserSettings();
        volumeSlider?.onValueChanged.AddListener(delegate { UpdateVolume(); });

        vsyncToggle?.onValueChanged.AddListener(delegate { SaveUserSettings(); });
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
        if (volumeSlider != null)
            volumeSlider.value = savedVol;

        if (musicSource != null)
            musicSource.volume = savedVol;

        int savedVSync = PlayerPrefs.GetInt("VSyncState", 1);
        if (vsyncToggle != null)
            vsyncToggle.isOn = (savedVSync == 1);

        QualitySettings.vSyncCount = savedVSync;
    }

    private void OnEnable()
    {
        // Subscribe to Unity's scene loaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Every time you load a new level...
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Find the new slider in the current level's hierarchy
        Slider newSlider = GameObject.Find("VolumeSlider")?.GetComponent<Slider>();
        Toggle newToggle = GameObject.Find("VSyncToggle")?.GetComponent<Toggle>();

        if (newSlider != null)
        {
            volumeSlider = newSlider; // Link it
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f); // Set saved value
                                                                             // Add the listener back so moving it still works
            volumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        }

        if (newToggle != null)
        {
            vsyncToggle = newToggle;
            vsyncToggle.isOn = PlayerPrefs.GetInt("VSyncState", 1) == 1;
            vsyncToggle.onValueChanged.AddListener(delegate { SaveUserSettings(); });
        }
    }
}