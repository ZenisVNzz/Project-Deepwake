using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "InitPlayFabTask", menuName = "StartupSystem/InitPlayFabTask")]
public class InitPlayFabTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        EventManager.Instance.Trigger("UI_NextProgress");
        IServiceRegistry SR = new ServiceRegistry();

        PlayFabServiceList playFabServiceList = ResourceManager.Instance.GetAsset<PlayFabServiceList>("PlayFabServiceList");

        foreach (PlayFabService service in playFabServiceList.Services)
        {
            string ServiceName = service.GetType().Name;
            Debug.Log($"[PlayFabService] Initialize service: {ServiceName}");
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

        var playFabServiceMg = new PlayFabServiceManager(SR);
        PlayFabClient playFabClient = playFabServiceMg.GetService<PlayFabClient>();
        await playFabClient.DefaultIdLoginAsync();

        await playFabServiceMg.InitAsync(serviceRegistry, ct);

        return true;
    }
}
