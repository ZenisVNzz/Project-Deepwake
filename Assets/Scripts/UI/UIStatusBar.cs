using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIStatusBar : MonoBehaviour, IRuntimeUI
{
    public Slider hpBar;
    public Slider mpBar;
    private Character _player;

    public void Initialize()
    {
        _player = FindObjectOfType<Character>();
        UpdateUI();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void UpdateUI()
    {
        hpBar.value = _player.CurrentHealth / _player.MaxHealth;
        mpBar.value = _player.CurrentStamina / _player.MaxStamina;
    }
}