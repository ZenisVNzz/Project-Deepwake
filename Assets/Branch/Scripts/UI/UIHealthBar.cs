using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar :  ProgressBarBase
{
    public void UpdateStamina(float current)
    {
        SetValue(current);
    }
}
