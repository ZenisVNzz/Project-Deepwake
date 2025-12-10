using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    public GameObject settingObject;
    public GameObject returnTitleButton;
    public GameObject exitGameButton;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown languageDropdown;
    public Toggle toggleFullScreen;

    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider vfxSlider;

    private Resolution[] resolutions;
    private static bool isFullScreen = true;
    private int defaultIndex;
    public static int selectedResolutionIndex;
    public static int selectedLanguageIndex;
    List<Resolution> selectedResolutions = new List<Resolution>();

    private float currentMasterVolume;
    private float currentMusicVolume;
    private float currentVFXVolume;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        currentMasterVolume = masterSlider.value;
        currentMusicVolume = musicSlider.value;
        currentVFXVolume = vfxSlider.value;

        toggleFullScreen.isOn = isFullScreen;

        if (!Application.isMobilePlatform)
        {
            resolutions = Screen.resolutions;

            List<string> resolutionListString = new List<string>();
            string newRes;
            foreach (Resolution res in resolutions)
            {
                newRes = res.width.ToString() + " x " + res.height.ToString();
                if (!resolutionListString.Contains(newRes))
                {
                    resolutionListString.Add(newRes);
                    selectedResolutions.Add(res);
                }
                if (res.width == Screen.currentResolution.width &&
                    res.height == Screen.currentResolution.height)
                {
                    defaultIndex = resolutionListString.Count - 1;
                }
            }

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resolutionListString);

            resolutionDropdown.value = defaultIndex;
            resolutionDropdown.RefreshShownValue();
        }
        else
        {
            resolutionDropdown.transform.parent.gameObject.SetActive(false);
            toggleFullScreen.transform.parent.gameObject.SetActive(false);
        }    

        languageDropdown.ClearOptions();
        List<string> languageList = new List<string> { "English", "Vietnamese" };
        languageDropdown.AddOptions(languageList);
    }

    public void OnEnable()
    {
        masterSlider.value = currentMasterVolume;
        musicSlider.value = currentMusicVolume;
        vfxSlider.value = currentVFXVolume;
        //if (SceneManager.GetActiveScene().name != "TITLESCREEN")
        //{
        //    returnTitleButton.SetActive(true);
        //    exitGameButton.SetActive(true);
        //}
        //else
        //{
        //    returnTitleButton.SetActive(false);
        //    exitGameButton.SetActive(false);
        //}    
    }

    public void ChangeRes()
    {
        selectedResolutionIndex = resolutionDropdown.value;
        Screen.SetResolution(selectedResolutions[selectedResolutionIndex].width,
                           selectedResolutions[selectedResolutionIndex].height,
                           isFullScreen);
    }

    public void ChangeFullScreen()
    {
        isFullScreen = toggleFullScreen.isOn;
        Screen.SetResolution(selectedResolutions[selectedResolutionIndex].width,
                           selectedResolutions[selectedResolutionIndex].height,
                           isFullScreen);
    }

    public void ChangeLanguage()
    {
        selectedLanguageIndex = languageDropdown.value;
        if (selectedLanguageIndex == 0)
        {
            Locale selectedLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
        else if (selectedLanguageIndex == 1)
        {
            Locale selectedLocale = LocalizationSettings.AvailableLocales.GetLocale("vi");
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
    }

    public void ReturnToTitleScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnExitGame()
    {
        Invoke("ExitGame", 0.5f);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetMasterVolume(float volume)
    {
        if (audioMixer == null) return;

        if (volume == -30f)
        {
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            audioMixer.SetFloat("Master", volume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioMixer == null) return;

        if (volume == -30f)
        {
            audioMixer.SetFloat("BGN", -80f);
        }
        else
        {
            audioMixer.SetFloat("BGM", volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (audioMixer == null) return;

        if (volume == -30f)
        {
            audioMixer.SetFloat("SFX", -80f);

        }
        else
        {
            audioMixer.SetFloat("SFX", volume);
        }
    }

    public void ApplySettings()
    {
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(vfxSlider.value);
        currentMasterVolume = masterSlider.value;
        currentMusicVolume = musicSlider.value;
        currentVFXVolume = vfxSlider.value;

        ChangeRes();
        ChangeFullScreen();
        ChangeLanguage();
    }
}
