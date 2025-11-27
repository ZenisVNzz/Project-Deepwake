using EasyTextEffects;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

public class ProgressText : MonoBehaviour
{
    [SerializeField] private List<LocalizedString> _textList;
    [SerializeField] private TextEffect _textEffect;
    [SerializeField] private GameObject _loadingUI;
    private int _currentIndex = 0;

    private async void Start()
    {
        await RegisterEvent();
    }

    public void NextProgress()
    {
        LocalizationText localizationText = GetComponent<LocalizationText>();
        if (localizationText != null && _textList.Count > 0)
        {
            localizationText.ChangeText(_textList[_currentIndex + 1]);
            _currentIndex = _currentIndex + 1;
        }
        else
        {
            Debug.LogError($"[ProgressText] LocalizationText component missing or text list is empty on object {gameObject.name}.");
        }

        if (_currentIndex >= _textList.Count - 1)
        {
            if (_loadingUI != null)
            {
                _loadingUI.SetActive(false);
                _textEffect.StartManualEffects();
            }
            else
            {
                Debug.LogError($"[ProgressText] Loading UI reference is missing on object {gameObject.name}.");
            }
        }
    }

    private async Task RegisterEvent()
    {
        while (EventManager.Instance == null)
        {
            await Task.Yield();
        }

        EventManager.Instance.Register("UI_NextProgress", NextProgress);
    }    
}
