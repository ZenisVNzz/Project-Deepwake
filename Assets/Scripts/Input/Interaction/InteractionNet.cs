using Mirror;
using UnityEngine;

public class InteractionNet : NetworkBehaviour
{
    private IInteractable currentInteract;

    public void SetCurrentInteract(IInteractable interactable)
    {
        currentInteract = interactable;
    }

    [Command]
    public void RequestEnter(NetworkIdentity target)
    {
        var interactable = target.GetComponentInChildren<IInteractable>();
        if (interactable == null) return;

        interactable.OnEnter(connectionToClient);
    }

    [Command]
    public void RequestExit(NetworkIdentity target)
    {
        var interactable = target.GetComponentInChildren<IInteractable>();
        if (interactable == null) return;

        interactable.OnExit(connectionToClient);
    }

    [Command]
    public void RequestInteract(NetworkIdentity target)
    {
        var interactable = target.GetComponentInChildren<IInteractable>();
        if (interactable == null) return;

        interactable.OnInteract(connectionToClient);
    }
}
