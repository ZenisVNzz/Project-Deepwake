using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusBar : MonoBehaviour, IRuntimeUIService
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Slider mpBar;
    [SerializeField] private TextMeshProUGUI mpText;

    private IPlayerRuntime _player;

    public void Initialize()
    {
        Hide();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void BindData(IPlayerRuntime data)
    {
        if (_player != null)
            _player.OnStatusChanged -= UpdateUI; 

        _player = data;

        if (_player != null)
            _player.OnStatusChanged += UpdateUI; 

        UpdateUI();
    }

    private void OnDisable()
    {
        if (_player != null)
            _player.OnStatusChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        if (_player == null) return;

        hpBar.value = _player.HP / _player.TotalHealth;
        hpText.text = $"{FormatNumber(_player.HP)} / {FormatNumber(_player.TotalHealth)}";

        mpBar.value = _player.Stamina / _player.TotalStamina;
        mpText.text = $"{FormatNumber(_player.Stamina)} / {FormatNumber(_player.TotalStamina)}";
    }

    private string FormatNumber(float value)
    {
        return Mathf.Approximately(value % 1f, 0f)
            ? value.ToString("0")
            : value.ToString("0.0");
    }
}