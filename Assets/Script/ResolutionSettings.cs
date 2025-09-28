using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    int[,] resolutions = new int[,]
    {
        {1024, 600},
        {1280, 720},
        {1600, 900},
        {1920, 1080}
    };

    void Start()
    {
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    void SetResolution(int index)
    {
        bool isFullscreen = Screen.fullScreen;
        Screen.SetResolution(resolutions[index,0], resolutions[index,1], isFullscreen);
    }

    void SetFullscreen(bool isFullscreen)
    {
        int index = resolutionDropdown.value;
        Screen.SetResolution(resolutions[index,0], resolutions[index,1], isFullscreen);
    }
}
