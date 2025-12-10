using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DeepwakeNetworkManager;

public class ClientHandler : MonoBehaviour
{
    public static ClientHandler Instance;
    private bool isLoadDone = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendLoadDone()
    {
        isLoadDone = true;
        NotifyServerLoadDone();
    }

    private void NotifyServerLoadDone()
    {
        if (NetworkClient.connection != null && NetworkClient.isConnected)
        {
            NetworkClient.Send(new ClientLoadDoneMessage());
        }
    }

    private void OnEnable() =>
        SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable() =>
        SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            Debug.Log("Client finished loading Game scene → notifying server");
            NetworkClient.Send(new DeepwakeNetworkManager.ClientReadyMessage());
        }
    }
}
