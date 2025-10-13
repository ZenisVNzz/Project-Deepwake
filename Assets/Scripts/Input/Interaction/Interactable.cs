using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private List<Action<GameObject>> registeredPlayerAction = new List<Action<GameObject>>();
    private List<Action> registeredAction = new List<Action>();

    public void OnEnter(GameObject player)
    {

    }

    public void OnExit(GameObject player)
    {

    }

    public void OnInteract(GameObject player)
    {
        if (registeredPlayerAction != null)
        {
            foreach (var action in registeredPlayerAction)
            {
                action?.Invoke(player);
            }
        }
        if (registeredAction != null)
        {
            foreach (var action in registeredAction)
            {
                action?.Invoke();
            }
        }      
    }

    public void Register(Action<GameObject> action)
    {
        registeredPlayerAction.Add(action);
    }

    public void Register(Action action)
    {
        registeredAction.Add(action);
    }
}
