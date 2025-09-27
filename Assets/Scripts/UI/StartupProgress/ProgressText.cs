using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class ProgressText : MonoBehaviour
{
    [SerializeField] private List<LocalizedString> _textList;
    private int _currentIndex = 0;

    public void NextProgress()
    {
        LocalizationText localizationText = GetComponent<LocalizationText>();
        if (localizationText != null && _textList.Count > 0)
        {
            localizationText.ChangeText(_textList[_currentIndex]);
            _currentIndex = (_currentIndex + 1) % _textList.Count;
        }
        else
        {
            Debug.LogError($"[ProgressText] LocalizationText component missing or text list is empty on object {gameObject.name}.");
        }
    }
}
