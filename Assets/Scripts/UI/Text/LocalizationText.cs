using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class LocalizationText : MonoBehaviour, ITextProvider
{
    [SerializeField] private LocalizedString _localizedString;

    private void Awake()
    {
        InitText();
    }

    public void InitText()
    {
        TextMeshProUGUI textComponent = GetComponent<TextMeshProUGUI>();

        if (textComponent == null)
        {
            Debug.LogError($"[LocalizationText] TextMeshProUGUI component missing on object {gameObject.name}.");
            return;
        }

        _localizedString.StringChanged += (localizedString) =>
        {
            textComponent.text = localizedString;
        };
    }

    public void SetArguments(List<string> value)
    {
        _localizedString.Arguments = value.ToArray();
    }
}
