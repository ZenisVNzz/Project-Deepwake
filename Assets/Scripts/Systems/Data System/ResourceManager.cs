using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private Dictionary<string, AsyncOperationHandle> _loadedAssets = new Dictionary<string, AsyncOperationHandle>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }    
        else
        {
            Destroy(this);
        }    
    }

    public async Task Preload(string label)
    {
        AsyncOperationHandle<IList<Object>> handle = Addressables.LoadAssetAsync<IList<Object>>(label);
        await handle.Task;
        
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {           
            foreach (Object asset in handle.Result)
            {
                if (!_loadedAssets.ContainsKey(asset.name))
                {
                    _loadedAssets.Add(asset.name, handle);
                    Debug.Log($"[ResourceManager] Preloaded asset {asset.name}");
                }    
            }    
        }    
        else
        {
            Debug.LogError($"[ResourceManager] Failed to load assets with label: {label}");
        }    
    }    
}
