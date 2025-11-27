using Mirror;
using UnityEngine;
using static DeepwakeNetworkManager;

public class ClientHandler : NetworkBehaviour
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
}
