using UnityEngine;

public class UIInventory : MonoBehaviour, IRuntimeUI
{
    public GameObject panel;

    public void Initialize() { Hide(); }

    public void Show() => panel.SetActive(true);

    public void Hide() => panel.SetActive(false);

    public void UpdateUI()
    {
        
    }
    public void BindData(object data)
    {

    }
}