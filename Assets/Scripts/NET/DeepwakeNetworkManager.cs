using Mirror;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeepwakeNetworkManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Client connected with ID: " + conn.connectionId);
        conn.Send(new LoadAssetMessage
        {
            sceneName = "Loading"
        });
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<ClientLoadDoneMessage>(OnClientLoadDone);
    }

    public struct LoadAssetMessage : NetworkMessage
    {
        public string sceneName;
        public SceneOperation sceneOperation;
        public bool customHandling;
    }

    public void SetupClient()
    {
        NetworkClient.RegisterHandler<LoadAssetMessage>(OnClientLoadAsset, false);
    }

    void OnClientLoadAsset(LoadAssetMessage msg)
    {
         _ = SceneLoader.Instance.LoadScene(msg.sceneName, true);
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (string.IsNullOrWhiteSpace(newSceneName))
        {
            Debug.LogError("ServerChangeScene empty scene name");
            return;
        }

        if (NetworkServer.isLoadingScene && newSceneName == networkSceneName)
        {
            Debug.LogError($"Scene change is already in progress for {newSceneName}");
            return;
        }

        if (!NetworkServer.active && newSceneName != offlineScene)
        {
            Debug.LogError("ServerChangeScene can only be called on an active server.");
            return;
        }

        NetworkServer.SetAllClientsNotReady();
        //networkSceneName = newSceneName;

        OnServerChangeScene(newSceneName);

        NetworkServer.isLoadingScene = true;

        loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);

        startPositionIndex = 0;
        startPositions.Clear();
    }

    public struct ClientLoadDoneMessage : NetworkMessage
    {
    }

    private void OnClientLoadDone(NetworkConnectionToClient conn, ClientLoadDoneMessage msg)
    {
        conn.Send(new SceneMessage()
        {
            sceneName = "Game"
        });

        Debug.Log("Client joined");

        if (!NetworkClient.ready)
        {
            NetworkClient.Ready();
        }

        if (IsRemoteClient(conn))
        {
            Debug.Log("OnServerAddPlayer called on server");
            if (conn.identity == null)
            {
                GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                NetworkServer.AddPlayerForConnection(conn, player);
                NetworkServer.SpawnObjects();
            }
        }

        if (NetworkServer.active && NetworkClient.connection != null && SceneManager.GetActiveScene().name != "Game")
        {
            singleton.ServerChangeScene("Game");
            StartCoroutine(WaitForGameSceneLoaded());
            return;
        }
    }

    private bool IsRemoteClient(NetworkConnectionToClient conn)
    {
        return conn != NetworkServer.localConnection;
    }

    private IEnumerator WaitForGameSceneLoaded()
    {
        while (SceneManager.GetActiveScene().name != "Game")
        {
            yield return null;
        }

        SpawnLocalHostPlayer();
    }

    private void SpawnLocalHostPlayer()
    {
        if (NetworkClient.connection.identity == null)
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(NetworkServer.localConnection, player);
            NetworkServer.SpawnObjects();
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("OnServerAddPlayer called on server");
        if (conn.identity == null)
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player);
            NetworkServer.SpawnObjects();
        }
    }
}
