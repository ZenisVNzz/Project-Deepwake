using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour, IManager
{
    public static ResourceManager Instance;
    private Dictionary<string, AssetReferences> _assetReferences = new Dictionary<string, AssetReferences>();
    private Dictionary<string, AsyncOperationHandle> _loadedAssets = new Dictionary<string, AsyncOperationHandle>();

    public async Task<bool> InitAsync()
    {
        AsyncOperationHandle<AssetReferencesList> handle = Addressables.LoadAssetAsync<AssetReferencesList>("AssetReferencesList");
        await handle.Task;
        AssetReferencesList AssetReferencesList = handle.Result;

        foreach (AssetReferences assetReference in AssetReferencesList.References)
        {
            _assetReferences[assetReference.Key] = assetReference;
        }

        return true;
    }

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

    public async Task Preload(string key)
    {
        if (_assetReferences.TryGetValue(key, out AssetReferences assetReferences))
        {
            if (assetReferences.Assets.Count > 0)
            {
                foreach (AssetReference assetReference in assetReferences.Assets)
                {
                    AsyncOperationHandle<Object> handle = assetReference.LoadAssetAsync<Object>();
                    await handle.Task;

                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        _loadedAssets[assetReference.Asset.name] = handle;
                        Debug.Log($"[ResourceManager] Preloaded asset: {assetReference.Asset.name}");
                    }    
                    else
                    {
                        Debug.LogError($"[ResourceManager] Preload asset: {assetReference.Asset.name} failed.");
                    }    
                }    
            }

            if (assetReferences.Labels.Count > 0)
            {
                foreach (AssetLabelReference labelReference in assetReferences.Labels)
                {
                    var locations = await Addressables.LoadResourceLocationsAsync(labelReference).Task;
                    var handles = new List<AsyncOperationHandle<Object>>();

                    foreach (var loc in locations)
                    {
                        AsyncOperationHandle<Object> handle = Addressables.LoadAssetAsync<Object>(loc);
                        handles.Add(handle);

                        await handle.Task;

                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            _loadedAssets[loc.PrimaryKey] = handle;
                            Debug.Log($"[ResourceManager] Preloaded asset: {loc.PrimaryKey}");
                        }    
                        else
                        {
                            Debug.LogError($"[ResourceManager] Preload asset: {loc.PrimaryKey} failed.");
                        }    
                    }
                }    
            }
        }
        else
        {
            Debug.LogError($"[ResourceManager] Preload assets with key: {key} failed.");
        }
    }
    
    public T GetAsset<T>(string key) where T : Object
    {
        if (_loadedAssets.TryGetValue(key, out AsyncOperationHandle handle))
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result as T;
            }
            else
            {
                Debug.LogError($"[ResourceManager] Asset with key: {key} not loaded successfully.");
            }
        }
        else
        {
            Debug.LogError($"[ResourceManager] Asset with key: {key} not found in cache.");
        }
        return null;
    }

    public void Release(string key)
    {
        if (_loadedAssets.TryGetValue(key, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);
            _loadedAssets.Remove(key);
            Debug.Log($"[ResourceManager] Released asset: {key}");
        }
        else
        {
            Debug.LogWarning($"[ResourceManager] No asset loaded with key: {key}");
        }
    }

    public void ReleaseAssetReferences(string key)
    {
        if (_assetReferences.TryGetValue(key, out AssetReferences assetReferences))
        {
            foreach (var assetReference in assetReferences.Assets)
            {
                string runtimeKey = assetReference.Asset.name;
                assetReference.ReleaseAsset();
                _loadedAssets.Remove(runtimeKey);
                Debug.Log($"[ResourceManager] Released asset: {runtimeKey}");
            }

            foreach (var labelReference in assetReferences.Labels)
            {
                var locations = Addressables.LoadResourceLocationsAsync(labelReference).WaitForCompletion();
                foreach (var loc in locations)
                {
                    string locKey = loc.PrimaryKey;
                    if (_loadedAssets.TryGetValue(locKey, out AsyncOperationHandle handle))
                    {
                        Addressables.Release(handle);
                        _loadedAssets.Remove(locKey);
                        Debug.Log($"[ResourceManager] Released asset from label: {locKey}");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning($"[ResourceManager] No AssetReferences found with key: {key}");
        }
    }

    //

    public static T LoadResource<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
