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
        _player = data;
        _player.OnHPChanged += OnHPChanged;
        _player.OnStaminaChanged += OnStaminaChanged;
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnHPChanged -= OnHPChanged;
            _player.OnStaminaChanged -= OnStaminaChanged;
        }
    }

    public void UpdateUI()
    {
        if (_player == null) return;

        hpBar.value = _player.HP / _player.TotalHealth;
        hpText.text = $"{FormatNumber(_player.HP)} / {FormatNumber(_player.TotalHealth)}";

        mpBar.value = _player.Stamina / _player.TotalStamina;
        mpText.text = $"{FormatNumber(_player.Stamina)} / {FormatNumber(_player.TotalStamina)}";
    }

    private void OnHPChanged(float newHp)
    {
        if (_player == null) return;
        hpBar.value = newHp / _player.TotalHealth;
        hpText.text = $"{FormatNumber(newHp)} / {FormatNumber(_player.TotalHealth)}";
    }

    private void OnStaminaChanged(float newStamina)
    {
        if (_player == null) return;
        mpBar.value = newStamina / _player.TotalStamina;
        mpText.text = $"{FormatNumber(newStamina)} / {FormatNumber(_player.TotalStamina)}";
    }

    private string FormatNumber(float value)
    {
        return Mathf.Approximately(value % 1f, 0f)
            ? value.ToString("0")
            : value.ToString("0.0");
    }
}