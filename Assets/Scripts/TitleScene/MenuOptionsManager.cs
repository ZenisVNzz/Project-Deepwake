using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionsManager : MonoBehaviour
{
    public Button SingleplayerButton;
    public Button MultiplayerButton;
    public Button SettingsButton;
    public Button ExitButton;

    public GameObject MultiplayerOptionsPanel;
    public GameObject SettingsPanel;

    private void Start()
    {
        SingleplayerButton.onClick.AddListener(OnSingleplayerButtonClicked);
        MultiplayerButton.onClick.AddListener(OnMultiplayerButtonClicked);
        SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private async void OnSingleplayerButtonClicked()
    {
        NetworkManager.singleton.StartHost();
        await SceneLoader.Instance.LoadScene("Game", true);
    }

    private void OnMultiplayerButtonClicked()
    {
        MultiplayerOptionsPanel.SetActive(true);
    }

    private void OnSettingsButtonClicked()
    {
        SettingsPanel.SetActive(true);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
