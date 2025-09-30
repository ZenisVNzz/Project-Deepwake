using UnityEngine;

public class GameVersionUI : MonoBehaviour
{
    private void Awake()
    {
        LocalizationText localizationText = GetComponent<LocalizationText>();
        if (localizationText != null)
        {
            localizationText.SetArguments(new System.Collections.Generic.List<string> { Application.version });
        }
        else
        {
            Debug.LogError($"[GameVersionUI] LocalizationText component missing on object {gameObject.name}.");
        }
    }
}
