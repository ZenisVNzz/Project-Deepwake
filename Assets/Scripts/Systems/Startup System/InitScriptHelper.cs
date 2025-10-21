using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class InitScriptHelper : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _manager;
    [SerializeField] private List<string> _preloadKeys;
    [SerializeField] private string _loadSceneOnLoadDone;

    private SceneLoader _sceneLoader;

    private async void Awake()
    {
        _sceneLoader = GetComponent<SceneLoader>();

        int totalTask = _manager.Count;
        int taskDone = 0;
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

                while (!initTask.IsCompleted)
                {
                    Assert.IsTrue(initTask.Result, $"{manager.GetType().Name} failed to initialize.");
                    await Task.Yield();
                }          
                taskDone++;
            }
            else
            {
                Debug.LogError($"{manager.GetType().Name} does not implement IManager interface.");
            }
        }
        
        if (taskDone  == totalTask)
        {
            await LoadScene();
        }
    }

    private async Task LoadAsset()
    {
        foreach (var key in _preloadKeys)
        {
            var resourceManager = ResourceManager.Instance;
            if (resourceManager != null)
            {
                await resourceManager.Preload(key);
            }
            else
            {
                Debug.LogError("ResourceManager instance is null.");
            }
        }
    }

    private async Task LoadScene()
    {   
        await _sceneLoader.LoadScene(_loadSceneOnLoadDone);
    }
}
