using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InitScriptHelper : MonoBehaviour
{
    [SerializeField] public List<MonoBehaviour> _manager;
    [SerializeField] public bool LoadAssetWhenInitResourceManager = false;
    [SerializeField] public List<string> _preloadKeys;
    [SerializeField] public string _loadSceneOnLoadDone;  

    private SceneLoader _sceneLoader;

    public async void Load()
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
            Debug.Log("[InitScriptHelper] All assets preloaded successfully.");
            await LoadScene();
        }
    }

    private async Task LoadScene()
    {
        if (_loadSceneOnLoadDone != "Loading")
        {
            await _sceneLoader.LoadScene(_loadSceneOnLoadDone, false);
        }
        else
        {
            ClientHandler.Instance.SendLoadDone();
        }
    }
}
