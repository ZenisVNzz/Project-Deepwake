using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetValue(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
