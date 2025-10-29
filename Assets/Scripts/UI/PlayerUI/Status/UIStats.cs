using UnityEngine;
using UnityEngine.UI;

public class UIStats : MonoBehaviour
{
    [Header("Character Base Stats")]
    [SerializeField] private Text maxHealthText;
    [SerializeField] private Text maxStaminaText;
    [SerializeField] private Text attackPowerText;
    [SerializeField] private Text defenseText;
    [SerializeField] private Text speedText;
    [SerializeField] private Text criticalChanceText;
    [SerializeField] private Text criticalDamageText;

    [Header("Character Bonus Stats")]
    [SerializeField] private Text bonusMaxHealthText;
    [SerializeField] private Text bonusMaxStaminaText;
    [SerializeField] private Text bonusAttackPowerText;
    [SerializeField] private Text bonusDefenseText;
    [SerializeField] private Text bonusSpeedText;
    [SerializeField] private Text bonusCriticalChanceText;
    [SerializeField] private Text bonusCriticalDamageText;




    public void UpdateStats(CharacterRuntime stats)
    {
        //maxHealthText.text = $"Health: {stats.MaxHealth}";
        //maxStaminaText.text = $"Stamina: {stats.MaxStamina}";
        //attackPowerText.text = $"Attack: {stats.Attack}";
        //defenseText.text = $"Defense: {stats.Defense}";
        //speedText.text = $"Speed: {stats.Speed}";
        //criticalChanceText.text = $"Crit Chance: {stats.CriticalChance * 100}%";
        //criticalDamageText.text = $"Crit Damage: {stats.CriticalDamage * 100}%";
    }
}