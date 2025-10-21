using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private List<Action<GameObject>> registeredPlayerAction = new List<Action<GameObject>>();
    private List<Action> registeredAction = new List<Action>();

    private Outline currentOutline;

    public void OnEnter(GameObject player)
    {
        currentOutline = GetComponent<Outline>();
        currentOutline.ActiveOutline();
    }

    public void OnExit(GameObject player)
    {
        currentOutline.DeactiveOutline();
        currentOutline = null;
    }

    public void OnInteract(GameObject player)
    {
        if (registeredPlayerAction != null)
        {
            foreach (var action in registeredPlayerAction)
            {
                action?.Invoke(player);
                currentOutline.DeactiveOutline();
            }
        }
        if (registeredAction != null)
        {
            foreach (var action in registeredAction)
            {
                action?.Invoke();
                currentOutline.DeactiveOutline();
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
