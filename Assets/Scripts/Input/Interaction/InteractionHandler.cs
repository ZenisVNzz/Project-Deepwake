using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour, IInteractionHandler
{
    private IInteractable currentInteract;
    private GameObject mainGameObject;

    private void Awake()
    {
        mainGameObject = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteract = interactable;
            interactable.OnEnter(mainGameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && currentInteract == interactable)
        {
            interactable.OnExit(mainGameObject);
            currentInteract = null;
        }
    }

    public void Interact()
    {
        if (currentInteract != null)
        {
            currentInteract.OnInteract(mainGameObject);
        }
    }
}