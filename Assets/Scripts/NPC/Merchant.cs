using UnityEngine;

public class Merchant : MonoBehaviour
{
    private IInteractable interactable;

    private void Awake()
    {
        interactable = GetComponentInChildren<IInteractable>();
        interactable.Register(OnInteract);
        interactable.RegisterOnEnter(OnEnter);
        interactable.RegisterOnExit(OnExit);
    }

    private void OnInteract()
    {

    }

    private void OnEnter()
    {
        CameraOffset.Instance.Move(-3.5f);
    }

    private void OnExit()
    {
        CameraOffset.Instance.Move(0f);
    }
}
