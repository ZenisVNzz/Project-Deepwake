using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private UIInventory inventoryUI;

    private void Start()
    {
        inventoryUI.RefreshUI(playerInventory.slots);
    }

    private void Update()
    {

    }
}