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

    public override async Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
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
                        return StartupTaskResult.Fail("NET_SERVICE_INIT_FAILED", $"Failed to initialize network service: {ServiceName}");
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning($"[Network] Initialize timeout or cancelled: {ServiceName}");
                    return StartupTaskResult.Fail("NET_SERVICE_INIT_TIMEOUT", $"Network service initialization timed out: {ServiceName}");
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    return StartupTaskResult.Fail("NET_SERVICE_INIT_EXCEPTION", $"Exception while initializing network service: {ServiceName}");
                }
            }
        }

        var Net = new NetworkManager(SR);
        await Net.InitAsync(serviceRegistry, ct);

        return StartupTaskResult.Ok();
    }
}
