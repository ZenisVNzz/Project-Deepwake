using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ItemStock : MonoBehaviour
{
    [SerializeField] private LocalizationText itemName;
    [SerializeField] private TextMeshProUGUI priceIndex;
    [SerializeField] private TextMeshProUGUI quantity;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject slectedMask;

    [SerializeField] private TMP_ColorGradient commonColor;
    [SerializeField] private TMP_ColorGradient rareColor;
    [SerializeField] private TMP_ColorGradient epicColor;
    [SerializeField] private TMP_ColorGradient legendaryColor;

    private Button button;

    private ItemCategory itemData;
    public ItemCategory ItemData => itemData;

    private Shop shop;

    public void SetData(ItemCategory itemData, Shop shop)
    {
        this.shop = shop;

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

        quantity.text = itemData.stock.ToString();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnSelected);

        shop.OnCurrentItemChanged += MaskAsSelected;
        slectedMask.SetActive(false);
    }

    private void MaskAsSelected(ItemStock itemStock)
    {
        if (this == null || slectedMask == null)
            return;

        if (itemStock == this)
        {
            slectedMask.SetActive(true);
        }
        else
        {
            slectedMask.SetActive(false);
        }
    }

    private void OnSelected()
    {
        shop.SelectItem(this);
    }
}
