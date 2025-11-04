using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProgressBarBase : MonoBehaviour, IProgressBar
{
    [Header("UI References")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected TMP_Text valueText;

    protected float currentValue;
    protected float maxValue;

    public virtual void Initialize(float max)
    {
        maxValue = max;
        currentValue = max;

        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

        UpdateBar();
    }

    public virtual void SetValue(float current)
    {
        currentValue = Mathf.Clamp(current, 0, maxValue);
        UpdateBar();
    }

    public virtual void UpdateBar()
    {
        if (slider != null)
            slider.value = currentValue;

        if (valueText != null)
            valueText.text = $"{Mathf.RoundToInt(currentValue)} / {Mathf.RoundToInt(maxValue)}";
    }
}