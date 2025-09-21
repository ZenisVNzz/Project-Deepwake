using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class NetworkManager : IGameService
{
    private NetworkServiceList _networkServiceList;
    private ServiceRegistry _serviceRegistry;

    public async Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        AsyncOperationHandle<NetworkServiceList> handle = Addressables.LoadAssetAsync<NetworkServiceList>("NetworkServiceList");
        _networkServiceList = await handle.Task;

        foreach (var service in _networkServiceList.NetworkServices)
        {
            string ServiceName = service.GetType().Name;
            Debug.Log($"[Network] Initialize service: {ServiceName}");
            using (var timeoutCTS = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                timeoutCTS.CancelAfter(TimeSpan.FromSeconds(10));
                try
                {
                    var o = await service.InitAsync(serviceRegistry, timeoutCTS.Token);
                    if (!o)
                    {
                        Debug.Log($"[Network] Initialize service failed: {ServiceName}");
                        return false;
                    }    
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning($"[Network] Initialize timeout or cancelled: {ServiceName}");
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    return false;
                }
            }    
        }
        serviceRegistry.Register<NetworkManager>(this);
        return true;
    }    
}
