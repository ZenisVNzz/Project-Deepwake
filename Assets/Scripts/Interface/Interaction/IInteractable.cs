using System;
using UnityEngine;

public interface IInteractable
{
    public void SetActive();
    public void SetInactive();
    public void OnEnter(GameObject player);
    public void OnExit(GameObject player);
    public void OnInteract(GameObject player);
    public void Register(Action<GameObject> action);
    public void Register(Action action);
    public void RegisterOnEnter(Action<GameObject> action);
    public void RegisterOnEnter(Action action);
    public void RegisterOnExit(Action<GameObject> action);
    public void RegisterOnExit(Action action);
}
