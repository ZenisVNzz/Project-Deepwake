using UnityEngine;

public class ShopUI : MonoBehaviour, IRuntimeUIService
{
    [SerializeField] private GameObject ShopUIObject;
    private IPlayerRuntime playerRuntime;

    private void Awake()
    {
        UIManager.Instance.RuntimeUIServiceRegistry.Register(this);
    }

    public void Initialize()
    {
    }

    public void Show()
    {
        if (playerRuntime == null)
        {
            Debug.LogError("ShopUI: playerRuntime is null, cannot initialize");
            return;
        }

        ShopUIObject.SetActive(true);
    }

    public void Hide()
    {
        ShopUIObject.SetActive(false);
    }

    public void UpdateUI()
    {
    }

    public void BindData(IPlayerRuntime data)
    {
        playerRuntime = data;
    }
}
