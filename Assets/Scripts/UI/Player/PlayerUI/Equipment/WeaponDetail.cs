using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class WeaponDetail : MonoBehaviour
{
    [SerializeField] private LocalizationText weaponNameText;
    [SerializeField] private LocalizationText weaponTierText;
    [SerializeField] private TextMeshProUGUI weaponDamageText;

    [SerializeField] private TMP_ColorGradient commonColor;
    [SerializeField] private TMP_ColorGradient rareColor;
    [SerializeField] private TMP_ColorGradient epicColor;
    [SerializeField] private TMP_ColorGradient legendaryColor;

    [SerializeField] UIEquipmentSlot equipmentSlot;

    private void Start()
    {
        equipmentSlot.OnEquipWeapon += UpdateWeaponDetail;
        UpdateWeaponDetail(equipmentSlot.EquipmentData != null);
    }

    private void UpdateWeaponDetail(bool isEquipped)
    {
        if (isEquipped)
        {
            weaponNameText.ChangeText(equipmentSlot.EquipmentData.itemName);
            
            switch (equipmentSlot.EquipmentData.itemTier)
            {
                case ItemTier.Common:
                    weaponTierText.ChangeText(new LocalizedString("UI", "UI_Common"));
                    break;
                case ItemTier.Rare:
                    weaponTierText.ChangeText(new LocalizedString("UI", "UI_Rare"));
                    break;
                case ItemTier.Epic:
                    weaponTierText.ChangeText(new LocalizedString("UI", "UI_Epic"));
                    break;
                case ItemTier.Legendary:
                    weaponTierText.ChangeText(new LocalizedString("UI", "UI_Legendary"));
                    break;
            }

            weaponTierText.GetComponent<TextMeshProUGUI>().colorGradientPreset = equipmentSlot.EquipmentData.itemTier switch
            {
                ItemTier.Common => commonColor,
                ItemTier.Rare => rareColor,
                ItemTier.Epic => epicColor,
                ItemTier.Legendary => legendaryColor,
                _ => commonColor,
            };
            weaponDamageText.text = equipmentSlot.EquipmentData.attackPowerBonus.ToString();

            weaponNameText.transform.gameObject.SetActive(true);
            weaponTierText.transform.gameObject.SetActive(true);
            weaponDamageText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            weaponNameText.transform.gameObject.SetActive(false);
            weaponTierText.transform.gameObject.SetActive(false);
            weaponDamageText.transform.parent.gameObject.SetActive(false);
        }
    }
}
