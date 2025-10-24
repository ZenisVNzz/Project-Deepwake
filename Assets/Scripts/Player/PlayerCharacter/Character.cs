using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Base Stats")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float attackPower = 10;
    [SerializeField] private float defense = 5;
    [SerializeField] private float speed = 5;
    [SerializeField] private float criticalChance = 0.1f;
    [SerializeField] private float criticalDamage = 1.5f;

    [Header("Character Attributes")]
    [SerializeField] private float Vitality = 5;
    [SerializeField] private float Strength = 5;
    [SerializeField] private float Luck = 5;

    [Header("Character Bonus Stats")]
    public float bonusMaxHealth = 100;
    public float bonusStamina = 100;
    public float bonusAttackPower = 10;
    public float bonusDefense = 5;
    public float bonusSpeed = 5;
    public float bonusCriticalChance = 0.1f;
    public float bonusCriticalDamage = 1.5f;

    [Header("Character Bonus Attributes")]
    public float bonusVitality = 5;
    public float bonusStrength = 5;
    public float bonusLuck = 5;

    private UIStatusBar _statusBar;

    public float CurrentHealth { get; set; }
    public float CurrentStamina { get; set; }

    public float MaxHealth => maxHealth + bonusMaxHealth;
    public float MaxStamina => maxStamina + bonusStamina;
    public float Attack => attackPower + bonusAttackPower;
    public float Defense => defense + bonusDefense;
    public float Speed => speed + bonusSpeed;
    public float CriticalChance => criticalChance + bonusCriticalChance;
    public float CriticalDamage => criticalDamage + bonusCriticalDamage;

    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentStamina = MaxStamina;
        var uiManager = FindObjectOfType<UIManager>();
        _statusBar = uiManager.GetUI<UIStatusBar>("StatusBar");

        if (_statusBar != null)
        {
            _statusBar.BindData(this);
            _statusBar.Show();
        }
        uiManager.Initialize(MaxHealth, MaxStamina);
        uiManager.UpdateHealth(CurrentHealth);
        uiManager.UpdateStamina(CurrentStamina);
    }

    private void Update()
    {
        _statusBar?.UpdateUI();
    }

    public void TakeDamage(float dmg)
    {
        CurrentHealth -= dmg;
        uiManager.UpdateHealth(CurrentHealth);
    }

    public void UseStamina(float amount)
    {
        CurrentStamina -= amount;
        uiManager.UpdateStamina(CurrentStamina);
    }

    public void RecoverStamina(float rate)
    {
        CurrentStamina = Mathf.Min(CurrentStamina + rate * Time.deltaTime, MaxStamina);
        uiManager.UpdateStamina(CurrentStamina);
    }
}
