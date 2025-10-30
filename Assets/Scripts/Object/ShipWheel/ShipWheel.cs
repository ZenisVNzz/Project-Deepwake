using UnityEngine;

public class ShipWheel : MonoBehaviour
{
    [SerializeField] private GameObject map;
    private IInteractable Interactable;

    private void Awake()
    {
        Interactable = GetComponentInChildren<Interactable>();
        Interactable.Register(OpenMap);
    }

    private void OpenMap()
    {
        map.SetActive(true);
    }
}
