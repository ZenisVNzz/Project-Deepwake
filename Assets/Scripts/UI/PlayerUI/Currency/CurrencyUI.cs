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
            goldCurrencyText.text = ammount.ToString();
        }
    }
}
