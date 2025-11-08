using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : NetworkBehaviour, IInteractable
{
    private List<Action<NetworkConnectionToClient>> registeredPlayerAction = new List<Action<NetworkConnectionToClient>>();
    private List<Action> registeredAction = new List<Action>();

    private List<Action<NetworkConnectionToClient>> registeredPlayerActionEnter = new List<Action<NetworkConnectionToClient>>();
    private List<Action> registeredActionEnter = new List<Action>();

    private List<Action<NetworkConnectionToClient>> registeredPlayerActionExit = new List<Action<NetworkConnectionToClient>>();
    private List<Action> registeredActionExit = new List<Action>();

    private Outline currentOutline;

    private bool isActive = true;

    public void SetActive()
    {
        isActive = true;
    }

    public void SetInactive()
    {
        isActive = false;
    }

    public void OnEnter(NetworkConnectionToClient player)
    {
        if (isActive == false) return;
        ActiveOutline(player);

        if (registeredPlayerActionEnter != null)
        {
            foreach (var action in registeredPlayerActionEnter)
            {
                action?.Invoke(player);
            }
        }
        if (registeredActionEnter != null)
        {
            foreach (var action in registeredActionEnter)
            {
                action?.Invoke();
            }
        }
    }

    [TargetRpc]
    private void ActiveOutline(NetworkConnection target)
    {
        currentOutline = GetComponent<Outline>();
        currentOutline.ActiveOutline();
    }

    public void OnExit(NetworkConnectionToClient player)
    {
        if (isActive == false) return;
        DeactiveOutline(player);

        if (registeredPlayerActionExit != null)
        {
            foreach (var action in registeredPlayerActionExit)
            {
                action?.Invoke(player);
            }
        }
        if (registeredActionExit != null)
        {
            foreach (var action in registeredActionExit)
            {
                action?.Invoke();
            }
        }
    }

    [TargetRpc]
    private void DeactiveOutline(NetworkConnection target)
    {
        if (currentOutline != null)
        {
            currentOutline.DeactiveOutline();
            currentOutline = null;
        }
    }

    public void OnInteract(NetworkConnectionToClient player)
    {
        if (isActive == false) return;

        if (registeredPlayerAction != null)
        {
            foreach (var action in registeredPlayerAction)
            {
                action?.Invoke(player);
                DeactiveOutline(player);
            }
        }
        if (registeredAction != null)
        {
            foreach (var action in registeredAction)
            {
                action?.Invoke();
                DeactiveOutline(player);
            }
        }
    }

    public void Register(Action<NetworkConnectionToClient> action)
    {
        registeredPlayerAction.Add(action);
    }

    public void Register(Action action)
    {
        registeredAction.Add(action);
    }

    public void RegisterOnEnter(Action<NetworkConnectionToClient> action)
    {
        registeredPlayerActionEnter.Add(action);
    }

    public void RegisterOnEnter(Action action)
    {
        registeredActionEnter.Add(action);
    }

    public void RegisterOnExit(Action<NetworkConnectionToClient> action)
    {
        registeredPlayerActionExit.Add(action);
    }

    public void RegisterOnExit(Action action)
    {
        registeredActionExit.Add(action);
    }
}
