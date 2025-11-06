using System.Collections;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldCurrencyText;
    private IPlayerRuntime player;

    public void Bind(IPlayerRuntime runtime)
    {
        player = runtime;
        player.CurrencyWallet.OnCurrencyChanged += UpdateCurrencyUI;

        goldCurrencyText.text = player.CurrencyWallet.Get(CurrencyType.Gold).ToString();
    }

    private void UpdateCurrencyUI(CurrencyType type, int ammount)
    {
        if (type == CurrencyType.Gold && goldCurrencyText != null)
        {
            StartCoroutine(UpdateCurrencyAnimation(goldCurrencyText, ammount));
        }
    }

    private IEnumerator UpdateCurrencyAnimation(TextMeshProUGUI text, int ammount)
    {
        int startValue = int.Parse(text.text);
        int endValue = ammount;
        float duration = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            text.text = currentValue.ToString();
            yield return null;
        }
        text.text = endValue.ToString();
    }
}
