using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InitScriptHelper : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _manager;
    [SerializeField] private List<string> _preloadKeys;

    private async void Awake()
    {
        foreach (var manager in _manager)
        {
            if (manager is IManager initManager)
            {
                var initTask = initManager.InitAsync();
                await initTask;

                if (initManager is ResourceManager)
                {
                    await LoadAsset();
                }

                while (!initTask.IsCompleted) { }
                Assert.IsTrue(initTask.Result, $"{manager.GetType().Name} failed to initialize.");
            }
            else
            {
                Debug.LogError($"{manager.GetType().Name} does not implement IManager interface.");
            }
        }    
    }

    private async Task LoadAsset()
    {
        foreach (var key in _preloadKeys)
        {
            var resourceManager = ResourceManager.Instance;
            if (resourceManager != null)
            {
                await resourceManager.Preload("Startup");
            }
            else
            {
                Debug.LogError("ResourceManager instance is null.");
            }
        }
    }
}
