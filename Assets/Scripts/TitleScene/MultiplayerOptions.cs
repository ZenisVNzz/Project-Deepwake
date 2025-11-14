using Mirror;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerOptions : NetworkBehaviour
{
    public Button HostButton;
    public Button JoinButton;
    public TMP_InputField AddressInputField;

    private void Start()
    {
        HostButton.onClick.AddListener(OnHostButtonClicked);
        JoinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    private async void OnHostButtonClicked()
    {
        await WaitForHost();
    }

    private async Task WaitForHost()
    {
        NetworkManager.singleton.StartHost();
        float timeout = 5f;
        float timer = 0f;

        while ((!NetworkServer.active || !NetworkClient.isConnected) && timer < timeout)
        {
            await Task.Yield();
            timer += Time.deltaTime;
        }

        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Debug.Log("Host game successfully");
            await SceneLoader.Instance.LoadScene("Game", true);
        }
        else
        {
            Debug.Log("Host game failed");
        }
    }

    private void OnJoinButtonClicked()
    {
        NetworkManager.singleton.networkAddress = AddressInputField.text;
        StartCoroutine(ConnectToServer());
    }

    private IEnumerator ConnectToServer()
    {
        NetworkClient.Connect(NetworkManager.singleton.networkAddress);

        float timeout = 10f;
        float timer = 0f;

        while (!NetworkClient.isConnected && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (NetworkClient.isConnected)
        {
            Debug.Log("Connected to server successfully");
            yield return SceneLoader.Instance.LoadScene("Game", true);
        }
        else
        {
            Debug.Log("Failed to connect to server");
            NetworkManager.singleton.StopClient();
        }
    }
}
