using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionHandler : NetworkBehaviour, IInteractionHandler
{
    private List<IInteractable> currentInteract = new();
    private List<NetworkIdentity> currentTarget = new();
    private InteractionNet interactionNet;
    private bool Active = true;

    private void Awake()
    {
        interactionNet = GetComponentInParent<InteractionNet>();
    }

    public void SetActive() => Active = true;
    public void SetInactive() => Active = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer) return;

        if (collision.TryGetComponent(out IInteractable interactable))
        {
            currentInteract.Add(interactable);
            currentTarget.Add((interactable as MonoBehaviour).GetComponentInParent<NetworkIdentity>());
            interactionNet.RequestEnter((interactable as MonoBehaviour).GetComponentInParent<NetworkIdentity>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLocalPlayer) return;

        if (collision.TryGetComponent(out IInteractable interactable) && currentInteract.Contains(interactable))
        {
            currentInteract.Remove(interactable);
            currentTarget.Remove((interactable as MonoBehaviour).GetComponentInParent<NetworkIdentity>());
            interactionNet.RequestExit((interactable as MonoBehaviour).GetComponentInParent<NetworkIdentity>());
        }
    }

    public void Interact()
    {
        if (!Active || currentTarget == null || !isLocalPlayer) return;

        interactionNet.RequestInteract(currentTarget.Last());
    }
}