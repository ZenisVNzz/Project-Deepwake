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
        if (target == null)
        {
            Debug.LogWarning("[InteractionNet] RequestExit called with null target!");
            return;
        }

        var interactable = target.GetComponentInChildren<IInteractable>();
        if (interactable == null)
        {
            Debug.LogWarning($"[InteractionNet] RequestExit: No IInteractable found in {target.name}");
            return;
        }

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
