using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHandler : MonoBehaviour
{
    [SerializeField] private Button EquipButton;
    [SerializeField] private Button UnEquipButton;
    [SerializeField] private Button UseButton;
    [SerializeField] private Button DropButton;

    private UIInventory inventory;
    private UIInventorySlot currentSlot;
    private UIEquipmentSlot equipmentSlot;

    private void Start()
    {
        if (EquipButton != null)
        {
            EquipButton.onClick.AddListener(OnEquipClicked);
        }
        if (UnEquipButton != null)
        {
            UnEquipButton.onClick.AddListener(OnUnEquipClicked);
        }
        if (UseButton != null)
        {
            UseButton.onClick.AddListener(OnUseClicked);
        }
        if (DropButton != null)
        {
            DropButton.onClick.AddListener(OnDropClicked);
        }

        inventory = GetComponentInParent<UIInventory>();
        if (GetComponentInParent<UIInventorySlot>() != null )
        {
            currentSlot = GetComponentInParent<UIInventorySlot>();
        }
        else if (GetComponentInParent<UIEquipmentSlot>() != null)
        {
            equipmentSlot = GetComponentInParent<UIEquipmentSlot>();
        }
    }

    private void OnEquipClicked()
    {
        inventory.PlayerRuntime.PlayerEquipment.Equip(currentSlot.Slot.item);
        if (currentSlot != null)
        {
            inventory.PlayerRuntime.PlayerInventory.RemoveItem(currentSlot.Slot.item, 1);
            currentSlot.transform.GetComponent<ItemDetail>().DetailPanel.SetActive(false);
        }
    }

    private void OnUnEquipClicked()
    {
        inventory.PlayerRuntime.PlayerEquipment.Unequip(currentSlot.Slot.item);
        if (equipmentSlot != null)
        {
            equipmentSlot.SetEquipmentData(null);
            inventory.PlayerRuntime.PlayerInventory.AddItem(equipmentSlot.EquipmentData, 1);
            equipmentSlot.transform.GetComponent<ItemDetail>().DetailPanel.SetActive(false);
        }
    }

    private void OnUseClicked()
    {
    }

    private void OnDropClicked()
    {
    }
}
