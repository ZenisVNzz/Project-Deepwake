using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Display Settings")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    Resolution[] resolutions;

    private void Start()
    {
        // Lay danh sach do phan giai
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        // Load am thanh
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        //musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        //sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void ApplySettings()
    {
        // Cai dat am thanh
        AudioListener.volume = masterSlider.value;

        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        //PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        //PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        // Cai dat do phan giai
        int resolutionIndex = resolutionDropdown.value;
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, fullscreenToggle.isOn);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
