using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHandler : MonoBehaviour
{
    [SerializeField] private Button EquipButton;
    [SerializeField] private Button UnEquipButton;
    [SerializeField] private Button UseButton;
    [SerializeField] private Button DropButton;

    private UIInventory inventory;
    private UIEquipmentPanel equipmentPanel;

    [SerializeField] private UIInventorySlot currentSlot;
    [SerializeField] private UIEquipmentSlot equipmentSlot;

    private IPlayerRuntime playerRuntime;

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
        if (inventory == null)
        {
            equipmentPanel = GetComponentInParent<UIEquipmentPanel>();
            playerRuntime = equipmentPanel.PlayerRuntime;
        }
        else
        {
            playerRuntime = inventory.PlayerRuntime;
        }

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
        playerRuntime.PlayerEquipment.Equip(currentSlot.Slot.item);
        if (currentSlot != null)
        {
            playerRuntime.PlayerInventory.RemoveItem(currentSlot.Slot.item, 1);
            currentSlot.GetComponent<ItemDetail>().DetailPanel.SetActive(false);
        }
    }

    private void OnUnEquipClicked()
    {    
        if (equipmentSlot != null)
        {
            playerRuntime.PlayerInventory.AddItem(equipmentSlot.EquipmentData, 1);
            playerRuntime.PlayerEquipment.Unequip(equipmentSlot.EquipmentData);
            equipmentSlot.SetEquipmentData(null);
            equipmentSlot.GetComponent<ItemDetail>().DetailPanel.SetActive(false);
        }
    }

    private void OnUseClicked()
    {
    }

    private void OnDropClicked()
    {
    }
}
