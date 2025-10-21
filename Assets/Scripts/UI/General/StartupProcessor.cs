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
        var interactionPromptUI = FindObjectOfType<UIInteractionPrompt>();

        uiManager.RegisterUI("Inventory", inventory);
        uiManager.RegisterUI("StatusBar", statusBar);
        uiManager.RegisterUI("InteractionPrompt", interactionPromptUI);

        inventory.Initialize();
        statusBar.Initialize();
    }
}