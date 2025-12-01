using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsPopup : MonoBehaviour
{
    public Button openSettingButton;
    public Button returnToTitleButton;
    public Button QuitGameButton;

    public void Awake()
    {
        openSettingButton.onClick.AddListener(OnOpenSettings);
        returnToTitleButton.onClick.AddListener(OnReturnToTitle);
        QuitGameButton.onClick.AddListener(OnQuitGame);    
    }

    private void OnOpenSettings()
    {
        SettingManager.instance.settingObject.SetActive(true);
    }

    private void OnReturnToTitle()
    {
        _ = SceneLoader.Instance.LoadScene("TitleScene", false);
    }

    private void OnQuitGame()
    {
        Application.Quit();
    }
}
