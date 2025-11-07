using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InitScriptHelper : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _manager;
    [SerializeField] private bool LoadAssetWhenInitResourceManager = false;
    [SerializeField] private List<string> _preloadKeys;
    [SerializeField] private string _loadSceneOnLoadDone;  

    private bool isCompleted = false;
    private SceneLoader _sceneLoader;

    private async void Awake()
    {
        _sceneLoader = SceneLoader.Instance;

        int totalTask = _manager.Count;
        int taskDone = 0;
        foreach (var manager in _manager)
        {
            if (manager is IManager initManager)
            {
                var initTask = initManager.InitAsync();
                await initTask;

                if (LoadAssetWhenInitResourceManager && manager is ResourceManager)
                {
                    await LoadAsset();
                }

                while (!initTask.IsCompleted)
                {
                    await Task.Yield();
                }          
                taskDone++;
            }
            else
            {
                Debug.LogError($"{manager.GetType().Name} does not implement IManager interface.");
            }
        }

        if (!LoadAssetWhenInitResourceManager || taskDone == totalTask)
        {
            await LoadAsset();
        }
    }

    private async Task LoadAsset()
    {
        int totalPreload = _preloadKeys.Count;
        int preloadDone = 0;

        foreach (var key in _preloadKeys)
        {
            var resourceManager = ResourceManager.Instance;
            if (resourceManager != null)
            {
                await resourceManager.Preload(key);
                preloadDone++;
            }
            else
            {
                Debug.LogError("ResourceManager instance is null.");
            }
        }

        if (preloadDone == totalPreload)
        {
            isCompleted = true;
            Debug.Log("[InitScriptHelper] All assets preloaded successfully.");
            await LoadScene();
        }
    }

    private async Task LoadScene()
    {   
        await _sceneLoader.LoadScene(_loadSceneOnLoadDone);
    }
}
