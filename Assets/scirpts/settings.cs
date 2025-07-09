using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
{
    resolutions = Screen.resolutions;

    // Use a HashSet to avoid duplicate resolution entries
    HashSet<string> usedResolutions = new HashSet<string>();
    List<string> options = new List<string>();
    List<Resolution> uniqueResolutions = new List<Resolution>();

    int currentResolutionIndex = 0;

    for (int i = 0; i < resolutions.Length; i++)
    {
        string resString = resolutions[i].width + " x " + resolutions[i].height;

        if (!usedResolutions.Contains(resString))
        {
            usedResolutions.Add(resString);
            options.Add(resString);
            uniqueResolutions.Add(resolutions[i]);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = uniqueResolutions.Count - 1;
            }
        }
    }

    resolutions = uniqueResolutions.ToArray();

    resolutionDropdown.ClearOptions();
    resolutionDropdown.AddOptions(options);

    int savedResolutionIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
    resolutionDropdown.value = savedResolutionIndex;
    resolutionDropdown.RefreshShownValue();

    SetResolution(savedResolutionIndex);

    bool isFullscreen = PlayerPrefs.GetInt("fullscreen", Screen.fullScreen ? 1 : 0) == 1;
    fullscreenToggle.isOn = isFullscreen;
    Screen.fullScreen = isFullscreen;

    resolutionDropdown.onValueChanged.AddListener(SetResolution);
    fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
}

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
