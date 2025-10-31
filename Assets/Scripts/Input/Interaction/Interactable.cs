using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private List<Action<GameObject>> registeredPlayerAction = new List<Action<GameObject>>();
    private List<Action> registeredAction = new List<Action>();

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
    }

    public void OnExit(GameObject player)
    {
        if (isActive == false) return;
        currentOutline.DeactiveOutline();
        currentOutline = null;
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
}
