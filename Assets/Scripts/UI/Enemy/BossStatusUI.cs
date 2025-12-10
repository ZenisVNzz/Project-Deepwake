using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStatusUI : MonoBehaviour
{
    public Slider bossHealthBar;
    public Slider bossHealthVFXBar;
    public TextMeshProUGUI bossNameText;

    [SerializeField] private float vfxSpeed = 1.5f;
    private float targetValue;

    public void SetData(string bossName, float maxHealth)
    {
        bossNameText.text = bossName;
        bossHealthBar.maxValue = maxHealth;
        bossHealthVFXBar.maxValue = maxHealth;
        bossHealthBar.value = maxHealth;
        bossHealthVFXBar.value = maxHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        targetValue = currentHealth;
        bossHealthBar.value = currentHealth;
    }

    private void Update()
    {
        if (bossHealthVFXBar.value > bossHealthBar.value)
        {
            bossHealthVFXBar.value =
                Mathf.Lerp(bossHealthVFXBar.value, bossHealthBar.value, Time.deltaTime * vfxSpeed);
        }
        else
        {
            bossHealthVFXBar.value = bossHealthBar.value;
        }
    }
}
