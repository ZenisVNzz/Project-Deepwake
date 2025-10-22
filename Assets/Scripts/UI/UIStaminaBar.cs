using UnityEngine;
using UnityEngine.UI;

public class UIStaminaBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetValue(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}