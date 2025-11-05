using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    [SerializeField] private GameObject detailPanel;
    public GameObject DetailPanel => detailPanel;
    private bool isVisible = false;

    [SerializeField] private LocalizationText itemName;
    [SerializeField] private Image statIcon;
    [SerializeField] private TextMeshProUGUI statIndex;
    [SerializeField] private LocalizationText description;

    [SerializeField] private GameObject EquipButton;
    [SerializeField] private GameObject UnequipButton;
    [SerializeField] private GameObject UseButton;
    [SerializeField] private GameObject DropButton;

    [SerializeField] private Sprite atkIcon;
    [SerializeField] private Sprite defIcon;

    [SerializeField] private TMP_ColorGradient commonColor;
    [SerializeField] private TMP_ColorGradient rareColor;
    [SerializeField] private TMP_ColorGradient epicColor;
    [SerializeField] private TMP_ColorGradient legendaryColor;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ToggleDetail);
    }

    private void ToggleDetail()
    {
        if (GetComponent<UIInventorySlot>().Slot.item == null && GetComponent<UIEquipmentSlot>().EquipmentData == null) return;

        isVisible = !isVisible;
        detailPanel.SetActive(isVisible);
        if (detailPanel.activeSelf)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        ItemData itemData = GetComponent<UIInventorySlot>().Slot.item ? GetComponent<UIInventorySlot>().Slot.item : GetComponent<UIEquipmentSlot>().EquipmentData;
        itemName.ChangeText(itemData.itemName);
        itemName.GetComponent<TextMeshProUGUI>().colorGradientPreset = itemData.itemTier switch
        {
            ItemTier.Common => commonColor,
            ItemTier.Rare => rareColor,
            ItemTier.Epic => epicColor,
            ItemTier.Legendary => legendaryColor,
            _ => commonColor,
        };
        description.ChangeText(itemData.itemDescription);

        if (itemData.itemType == ItemType.Material)
        {
            statIcon.enabled = false;
            statIndex.enabled = false;
            EquipButton.SetActive(false);
            UseButton.SetActive(false);
            DropButton.SetActive(true);
        }
        else if (itemData.itemType == ItemType.Consumable)
        {
            statIcon.enabled = false;
            statIndex.enabled = false;
            EquipButton.SetActive(false);
            UseButton.SetActive(true);
            DropButton.SetActive(true);
        }
        else
        {
            if (itemData is EquipmentData equipmentData)
            {
                if (equipmentData.equipmentType == EquipmentType.Weapon)
                {
                    statIcon.sprite = atkIcon;
                    statIndex.text = equipmentData.attackPowerBonus.ToString();
                }
                else
                {
                    statIcon.sprite = defIcon;
                    statIndex.text = equipmentData.defenseBonus.ToString();
                }
            }

            statIcon.enabled = true;
            statIndex.enabled = true;
            if (EquipButton != null)
            {
                EquipButton.SetActive(true);
            }
            else
            {
                UnequipButton.SetActive(true);
            }
            UseButton.SetActive(false);
            DropButton.SetActive(true);
        }
    }
}
