using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ItemStock : MonoBehaviour
{
    [SerializeField] private LocalizationText itemName;
    [SerializeField] private TextMeshProUGUI priceIndex;
    [SerializeField] private Image itemImage;

    [SerializeField] private TMP_ColorGradient commonColor;
    [SerializeField] private TMP_ColorGradient rareColor;
    [SerializeField] private TMP_ColorGradient epicColor;
    [SerializeField] private TMP_ColorGradient legendaryColor;

    private ItemCategory itemData;
    public ItemCategory ItemData => itemData;

    public void SetData(ItemCategory itemData)
    {
        this.itemData = itemData;
        itemName.ChangeText(itemData.ItemData.itemName);
        itemName.GetComponent<TextMeshProUGUI>().colorGradientPreset = itemData.ItemData.itemTier switch
        {
            ItemTier.Common => commonColor,
            ItemTier.Rare => rareColor,
            ItemTier.Epic => epicColor,
            ItemTier.Legendary => legendaryColor,
            _ => commonColor,
        };

        priceIndex.text = itemData.price.ToString();
        itemImage.sprite = itemData.ItemData.icon;
    }
}
