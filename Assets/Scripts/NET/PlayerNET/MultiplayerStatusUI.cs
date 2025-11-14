using Mirror;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerStatusUI : MonoBehaviour
{
    public PlayerRuntime localPlayer;
    public List<UIStatusBar> statusBars = new List<UIStatusBar>();
    public List<PlayerRuntime> boundPlayer = new List<PlayerRuntime>();

    private void Start()
    {
        if (PlayerNetManager.Instance != null)
        {
            PlayerNetManager.Instance.OnNewPlayerJoined += BindData;
        }

        AutoBindData();
    }

    private void OnDestroy()
    {
        if (PlayerNetManager.Instance != null)
        {
            PlayerNetManager.Instance.OnNewPlayerJoined -= BindData;
        }   
    }

    public void BindLocalPlayer(PlayerRuntime playerRuntime)
    {
        localPlayer = playerRuntime;
        boundPlayer.Add(localPlayer);
    }

    public void BindData(PlayerRuntime playerData, bool state)
    {
        foreach (var statusBar in statusBars)
        {
            if (boundPlayer.Contains(playerData) || playerData == localPlayer || statusBar._player != null) continue;

            statusBar.BindData(playerData);
            statusBar.gameObject.SetActive(true);
            boundPlayer.Add(playerData);
            break;
        }
    }

    public void AutoBindData()
    {
        if (PlayerNetManager.Instance == null) return;

        var allPlayerRuntimes = PlayerNetManager.Instance.GetAllPlayerRuntimes();

        foreach (var statusBar in statusBars)
        {
            if (statusBar._player != null) continue;

            foreach (var playerRuntime in allPlayerRuntimes)
            {
                if (boundPlayer.Contains(playerRuntime) || playerRuntime == localPlayer) continue;

                statusBar.BindData(playerRuntime);
                statusBar.gameObject.SetActive(true);
                boundPlayer.Add(playerRuntime);
                break;
            }
        }
    }
}
