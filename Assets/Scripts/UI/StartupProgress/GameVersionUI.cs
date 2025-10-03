using UnityEngine;
using System.Collections.Generic;

public class GameVersionUI : MonoBehaviour
{
    private void Awake()
    {
        LocalizationText localizationText = GetComponent<LocalizationText>();
        if (localizationText != null)
        {
            localizationText.SetArguments(new List<string> { Application.version });
        }
        else
        {
            Debug.LogError($"[GameVersionUI] LocalizationText component missing on object {gameObject.name}.");
        }
    }
}
