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
        Hide();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void BindData(object data)
    {
        _player = data as Character;
    }

    public void UpdateUI()
    {
        if (_player == null) return;
        hpBar.value = _player.CurrentHealth / _player.MaxHealth;
        mpBar.value = _player.CurrentStamina / _player.MaxStamina;
    }
}