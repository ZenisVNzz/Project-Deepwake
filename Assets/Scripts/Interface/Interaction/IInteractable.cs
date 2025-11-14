using Mirror;
using System;
using UnityEngine;

public interface IInteractable
{
    public void SetActive();
    public void SetInactive();
    public void OnEnter(NetworkConnectionToClient player);
    public void OnExit(NetworkConnectionToClient player);
    public void OnInteract(NetworkConnectionToClient player);
    public void Register(Action<NetworkConnectionToClient> action);
    public void Register(Action action);
    public void RegisterOnEnter(Action<NetworkConnectionToClient> action);
    public void RegisterOnEnter(Action action);
    public void RegisterOnExit(Action<NetworkConnectionToClient> action);
    public void RegisterOnExit(Action action);
}
