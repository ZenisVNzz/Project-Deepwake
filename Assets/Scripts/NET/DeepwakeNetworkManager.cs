using Mirror;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeepwakeNetworkManager : NetworkManager
{
    public override void OnClientConnect()
    {
        StartCoroutine(WaitForGameSceneAndReady());
    }

    private IEnumerator WaitForGameSceneAndReady()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Game");
        Debug.Log("Game scene loaded, setting client ready");

        if (!NetworkClient.ready)
        {
            NetworkClient.Ready();
        }
            

        if (NetworkServer.active && NetworkClient.connection != null)
        {
            SpawnLocalHostPlayer();
        }
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
        }
    }
}
