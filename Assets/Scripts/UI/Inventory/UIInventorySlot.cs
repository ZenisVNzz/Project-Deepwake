using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    private InventorySlot slot;

    public void Bind(InventorySlot data)
    {
        slot = data;

        if (slot != null && !slot.IsEmpty)
        {
            icon.enabled = true;
            icon.sprite = slot.item.icon;
            quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
        }
        else
        {
            icon.enabled = false;
            quantityText.text = "";
        }
    }
}
