using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStats : MonoBehaviour
{
    [Header("Character Base Stats")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI maxStaminaText;
    [SerializeField] private TextMeshProUGUI attackPowerText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI criticalChanceText;
    [SerializeField] private TextMeshProUGUI criticalDamageText;

    [Header("Character Bonus Stats")]
    [SerializeField] private TextMeshProUGUI bonusMaxHealthText;
    [SerializeField] private TextMeshProUGUI bonusMaxStaminaText;
    [SerializeField] private TextMeshProUGUI bonusAttackPowerText;
    [SerializeField] private TextMeshProUGUI bonusDefenseText;
    [SerializeField] private TextMeshProUGUI bonusSpeedText;
    [SerializeField] private TextMeshProUGUI bonusCriticalChanceText;
    [SerializeField] private TextMeshProUGUI bonusCriticalDamageText;




    public void UpdateStats(PlayerRuntime stats)
    {
        levelText.text = stats.Level.ToString();
        maxHealthText.text = stats.TotalHealth == Mathf.FloorToInt(stats.TotalHealth) ? $"{Mathf.FloorToInt(stats.TotalHealth)}" : $"{stats.TotalHealth:0.0}";
        maxStaminaText.text = stats.TotalStamina == Mathf.FloorToInt(stats.TotalStamina) ? $"{Mathf.FloorToInt(stats.TotalStamina)}" : $"{stats.TotalStamina:0.0}";
        attackPowerText.text = stats.TotalAttack == Mathf.FloorToInt(stats.TotalAttack) ? $"{Mathf.FloorToInt(stats.TotalAttack)}" : $"{stats.TotalAttack:0.0}";
        defenseText.text = stats.TotalDefense == Mathf.FloorToInt(stats.TotalDefense) ? $"{Mathf.FloorToInt(stats.TotalDefense)}" : $"{stats.TotalDefense:0.0}";
        speedText.text = stats.TotalSpeed == Mathf.FloorToInt(stats.TotalSpeed) ? $"{Mathf.FloorToInt(stats.TotalSpeed)}" : $"{stats.TotalSpeed:0.0}";
        criticalChanceText.text = $"{stats.TotalCriticalChance}%";
        criticalDamageText.text = $"{stats.TotalCriticalDamage}x";

        bonusMaxHealthText.text = stats.BonusMaxHealth == 0f ? "" : $"(+{stats.BonusMaxHealth})";
        bonusMaxStaminaText.text = stats.BonusStamina == 0f ? "" : $"(+{stats.BonusStamina})";
        bonusAttackPowerText.text = stats.BonusAttackPower == 0f ? "" : $"(+{stats.BonusAttackPower})";
        bonusDefenseText.text = stats.BonusDefense == 0f ? "" : $"(+{stats.BonusDefense})";
        bonusSpeedText.text = stats.BonusSpeed == 0f ? "" : $"(+{stats.BonusSpeed})";
        bonusCriticalChanceText.text = stats.BonusCriticalChance == 0f ? "" : $"(+{stats.BonusCriticalChance}%)";
        bonusCriticalDamageText.text = stats.BonusCriticalDamage == 0f ? "" : $"(+{stats.BonusCriticalDamage}x)";
    }
}