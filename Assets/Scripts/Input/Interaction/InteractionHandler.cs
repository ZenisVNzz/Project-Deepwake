using Mirror;
using UnityEngine;

public class InteractionHandler : NetworkBehaviour, IInteractionHandler
{
    private IInteractable currentInteract;
    private NetworkIdentity currentTarget;
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
            currentInteract = interactable;
            currentTarget = (interactable as MonoBehaviour).GetComponentInParent<NetworkIdentity>();
            interactionNet.RequestEnter(currentTarget);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLocalPlayer) return;

        if (collision.TryGetComponent(out IInteractable interactable) && currentInteract == interactable)
        {
            currentInteract = null;
            currentTarget = null;
            interactionNet.RequestExit((interactable as MonoBehaviour).GetComponentInParent<NetworkIdentity>());
        }
    }

    public void Interact()
    {
        if (!Active || currentTarget == null || !isLocalPlayer) return;

        interactionNet.RequestInteract(currentTarget);
    }
}