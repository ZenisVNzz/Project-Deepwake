using Mirror;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetManager : NetworkBehaviour
{
    public static PlayerNetManager Instance;

    public PlayerRuntime localCharacterRuntime;

    public SyncList<PlayerRuntime> allCharacterRuntimes = new SyncList<PlayerRuntime>();

    public event Action<PlayerRuntime> OnNewPlayerJoined;
    public event Action<PlayerRuntime> OnPlayerLeft;

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
        allCharacterRuntimes.Callback += OnAllCharacterRuntimesChanged;

        foreach (var pr in allCharacterRuntimes)
        {
            OnNewPlayerJoined?.Invoke(pr);
        }
    }

    private void OnAllCharacterRuntimesChanged(SyncList<PlayerRuntime>.Operation op, int index, PlayerRuntime olditem, PlayerRuntime newitem)
    {
        switch (op)
        {
            case SyncList<PlayerRuntime>.Operation.OP_ADD:
                OnNewPlayerJoined?.Invoke(newitem);
                break;

            case SyncList<PlayerRuntime>.Operation.OP_REMOVEAT:
                OnPlayerLeft?.Invoke(newitem);
                break;
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
