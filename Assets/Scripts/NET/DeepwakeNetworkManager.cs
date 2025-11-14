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
        NetworkServer.RegisterHandler<ClientReadyMessage>(OnClientReady);
    }

    public struct LoadAssetMessage : NetworkMessage
    {
        public string sceneName;
        public SceneOperation sceneOperation;
        public bool customHandling;
    }

    public struct ClientLoadDoneMessage : NetworkMessage
    {
    }

    public struct ClientReadyMessage : NetworkMessage
    {
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

    private void OnClientLoadDone(NetworkConnectionToClient conn, ClientLoadDoneMessage msg)
    {
        conn.Send(new SceneMessage()
        {
            sceneName = "Game"
        });

        Debug.Log("Client joined");

        if (NetworkServer.active && NetworkClient.connection != null && SceneManager.GetActiveScene().name != "Game")
        {
            ServerChangeScene("Game");
        } 
    }

    private void OnClientReady(NetworkConnectionToClient conn, ClientReadyMessage msg)
    {
        if (!conn.isReady)
        {
            NetworkServer.SetClientReady(conn);
        }

        SpawnPlayer(conn);
    }

    private void SpawnPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
