using UnityEngine;

public class StartupProcessor : MonoBehaviour
{
    public UIManager uiManagerPrefab;
    private IServiceRegistry _registry;

    private void Awake()
    {
        _registry = new ServiceRegistry();


        var uiManager = Instantiate(uiManagerPrefab);
        _registry.RegisterService(uiManager);


        var inventory = FindObjectOfType<UIInventory>();
        var statusBar = FindObjectOfType<UIStatusBar>();

        uiManager.RegisterUI("Inventory", inventory);
        uiManager.RegisterUI("StatusBar", statusBar);

        inventory.Initialize();
        statusBar.Initialize();
    }
}