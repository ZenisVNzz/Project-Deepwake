using UnityEngine;

public class CannonController : MonoBehaviour
{
    private IInteractable Interactable;

    private void Awake()
    {
        Interactable = GetComponentInChildren<Interactable>();
        Interactable.Register(UseCannon);
    }

    private void UseCannon(GameObject player)
    {
        Debug.LogWarning("Use Cannon");
    }    
}
