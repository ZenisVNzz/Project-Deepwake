using UnityEngine;
using UnityEngine.UI;

public class UIStaminaBar : ProgressBarBase
{
    public void UpdateStamina(float current)
    {
        SetValue(current);
    }
}