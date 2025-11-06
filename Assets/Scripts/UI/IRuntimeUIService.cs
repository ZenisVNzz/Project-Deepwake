using UnityEngine;

public interface IRuntimeUIService
{
    void Initialize(); 
    void Show();         
    void Hide();         
    void UpdateUI();
    void BindData(IPlayerRuntime data);
}