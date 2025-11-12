using Mirror;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerNetManager : NetworkBehaviour
{
    public static PlayerNetManager Instance;

    public bool isOnlineMode => NetworkServer.active && NetworkServer.connections.Count >= 2;

    public PlayerRuntime localCharacterRuntime;

    public SyncList<PlayerRuntime> allCharacterRuntimes = new SyncList<PlayerRuntime>();
    public List<GameObject> allPlayerName = new List<GameObject>();

    public event Action<PlayerRuntime, bool> OnNewPlayerJoined;
    public event Action<PlayerRuntime, bool> OnPlayerLeft;

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

    public override void OnStartClient()
    {
        base.OnStartClient();

        allCharacterRuntimes.Callback += OnAllCharacterRuntimesChanged;
        OnNewPlayerJoined += OnPlayerConnectionChanged;
        OnPlayerLeft += OnPlayerConnectionChanged;

        foreach (var pr in allCharacterRuntimes)
        {
            OnNewPlayerJoined?.Invoke(pr, true);
        }
        SetActiveAllPlayerName(allPlayerName.Count >= 2);
    }

    private void OnPlayerConnectionChanged(PlayerRuntime playerRuntime, bool state)
    {
        if (playerRuntime == null) return;

        var nameGO = TryGetPlayerNameGO(playerRuntime);
        if (nameGO == null) return;

        if (state) // Joined
        {
            if (!allPlayerName.Contains(nameGO))
                allPlayerName.Add(nameGO);
        }
        else // Left
        {
            allPlayerName.Remove(nameGO);
        }

        SetActiveAllPlayerName(allPlayerName.Count >= 2);
    }

    private GameObject TryGetPlayerNameGO(PlayerRuntime pr)
    {
        var net = pr != null ? pr.GetComponent<PlayerNet>() : null;
        var label = net != null ? net.playerNameText : null;
        return label != null ? label.gameObject : null;
    }

    private void OnAllCharacterRuntimesChanged(SyncList<PlayerRuntime>.Operation op, int index, PlayerRuntime olditem, PlayerRuntime newitem)
    {
        switch (op)
        {
            case SyncList<PlayerRuntime>.Operation.OP_ADD:
                OnNewPlayerJoined?.Invoke(newitem, true);
                break;

            case SyncList<PlayerRuntime>.Operation.OP_REMOVEAT:
                OnPlayerLeft?.Invoke(newitem, false);
                break;
        }
    }

    public void SetActiveAllPlayerName(bool isActive)
    {
        foreach (var playerName in allPlayerName)
        {
            if (playerName != null)
            {
                playerName.SetActive(isActive);
            }
        }
    }

    public List<PlayerRuntime> GetAllPlayerRuntimes()
    {
        return new List<PlayerRuntime>(allCharacterRuntimes);
    }

    [Server]
    public void RegisterCharacterRuntime(PlayerRuntime characterRuntime)
    {
        if (!allCharacterRuntimes.Contains(characterRuntime))
        {
            allCharacterRuntimes.Add(characterRuntime);
        }
    }

    [Server]
    public void UnregisterCharacterRuntime(PlayerRuntime characterRuntime)
    {
        if (allCharacterRuntimes.Contains(characterRuntime))
        {
            allCharacterRuntimes.Remove(characterRuntime);
        }
    }
}
