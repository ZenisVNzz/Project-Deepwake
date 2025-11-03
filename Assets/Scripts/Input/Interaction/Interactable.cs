using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private List<Action<GameObject>> registeredPlayerAction = new List<Action<GameObject>>();
    private List<Action> registeredAction = new List<Action>();

    private List<Action<GameObject>> registeredPlayerActionEnter = new List<Action<GameObject>>();
    private List<Action> registeredActionEnter = new List<Action>();

    private List<Action<GameObject>> registeredPlayerActionExit = new List<Action<GameObject>>();
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

    public void OnEnter(GameObject player)
    {
        if (isActive == false) return;
        currentOutline = GetComponent<Outline>();
        currentOutline.ActiveOutline();

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

    public void OnExit(GameObject player)
    {
        if (isActive == false) return;
        currentOutline.DeactiveOutline();
        currentOutline = null;

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

    public void OnInteract(GameObject player)
    {
        if (isActive == false) return;

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

    public void RegisterOnEnter(Action<GameObject> action)
    {
        registeredPlayerActionEnter.Add(action);
    }

    public void RegisterOnEnter(Action action)
    {
        registeredActionEnter.Add(action);
    }

    public void RegisterOnExit(Action<GameObject> action)
    {
        registeredPlayerActionExit.Add(action);
    }

    public void RegisterOnExit(Action action)
    {
        registeredActionExit.Add(action);
    }
}
