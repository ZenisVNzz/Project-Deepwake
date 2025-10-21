using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInteractionPrompt : MonoBehaviour, IRuntimeUI
{
    [SerializeField] private TextMeshProUGUI promptText;

    public void Initialize() => Hide();
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    public void UpdateUI() { }
    public void BindData(object data) { }

    public void ShowPrompt(string text)
    {
        promptText.text = text;
        Show();
    }

    public void HidePrompt()
    {
        Hide();
    }
}