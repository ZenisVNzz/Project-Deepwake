using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "InitNetworkTask", menuName = "StartupSystem/InitNetworkTask")]
public class InitNetworkTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        EventManager.Instance.Trigger("UI_NextProgress");
        IServiceRegistry SR = new ServiceRegistry();

        NetworkServiceList networkServiceList = ResourceManager.Instance.GetAsset<NetworkServiceList>("NetworkServiceList");

        foreach (var service in networkServiceList.NetworkServices)
        {
            string ServiceName = service.GetType().Name;
            Debug.Log($"[Network] Initialize service: {ServiceName}");
            using (var timeoutCTS = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                timeoutCTS.CancelAfter(TimeSpan.FromSeconds(10));
                try
                {
                    var o = await service.InitAsync(SR, timeoutCTS.Token);
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

        var Net = new NetworkManager(SR);
        await Net.InitAsync(serviceRegistry, ct);

        return true;
    }
}
