using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    [SerializeField] private GameObject detailPanel;
    public GameObject DetailPanel => detailPanel;
    private bool isVisible => detailPanel.activeSelf;

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
        if (GetComponent<UIInventorySlot>() != null)
        {
            if (GetComponent<UIInventorySlot>().Slot.item == null)
                return;
        }
        else if (GetComponent<UIEquipmentSlot>() != null)
        {
            if (GetComponent<UIEquipmentSlot>().EquipmentData == null)
                return;
        }

        detailPanel.SetActive(!isVisible);
        if (detailPanel.activeSelf)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        ItemData itemData = GetComponent<UIInventorySlot>() ? GetComponent<UIInventorySlot>().Slot.item : GetComponent<UIEquipmentSlot>().EquipmentData;
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
            statIcon.transform.parent.gameObject.SetActive(false);
            statIndex.enabled = false;
            EquipButton.SetActive(false);
            UseButton.SetActive(false);
            DropButton.SetActive(true);
        }
        else if (itemData.itemType == ItemType.Consumable)
        {
            statIcon.transform.parent.gameObject.SetActive(true);
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

            statIcon.transform.parent.gameObject.SetActive(true);
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
