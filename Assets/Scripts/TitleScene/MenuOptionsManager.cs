using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionsManager : MonoBehaviour
{
    public Button SingleplayerButton;
    public Button MultiplayerButton;
    public Button SettingsButton;
    public Button ExitButton;
    public Button AccountCenterButton;

    public GameObject MultiplayerOptionsPanel;
    public GameObject SettingsPanel;

    public GameObject AccountCenterPanel;

    private void Start()
    {
        SingleplayerButton.onClick.AddListener(OnSingleplayerButtonClicked);
        MultiplayerButton.onClick.AddListener(OnMultiplayerButtonClicked);
        SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        AccountCenterButton.onClick.AddListener(OnAccountCenterClicked);
    }

    private void OnSingleplayerButtonClicked()
    {
        DeepwakeNetworkManager networkManager = FindAnyObjectByType<DeepwakeNetworkManager>();
        networkManager.SetupClient();
        NetworkManager.singleton.StartHost();
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

    private void OnAccountCenterClicked()
    {
        AccountCenterPanel.SetActive(true);
    }
}
