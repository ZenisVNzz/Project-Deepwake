using Mirror;
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

    private void OnInteract(NetworkConnectionToClient player)
    {
        ShopUI shopUI = UIManager.Instance.RuntimeUIServiceRegistry.Get<ShopUI>();
        shopUI.BindData(player.identity.gameObject.GetComponent<IPlayerRuntime>());
        shopUI.Show();
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
