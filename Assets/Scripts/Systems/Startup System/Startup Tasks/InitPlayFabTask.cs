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
    public override bool RequiresNetwork => true;
    public override bool isMainProgressTask => true;

    public override async Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
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
                        return StartupTaskResult.Fail("PLAYFAB_SERVICE_INIT_FAILED", $"Failed to initialize PlayFab service: {ServiceName}");
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning($"[Network] Initialize timeout or cancelled: {ServiceName}");
                    return StartupTaskResult.Fail("PLAYFAB_SERVICE_INIT_TIMEOUT", $"PlayFab service initialization timed out: {ServiceName}");
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    return StartupTaskResult.Fail("PLAYFAB_SERVICE_INIT_EXCEPTION", $"Exception while initializing PlayFab service: {ServiceName}");
                }
            }
        }

        var playFabServiceMg = new PlayFabServiceManager(SR);
        PlayFabClient playFabClient = playFabServiceMg.GetService<PlayFabClient>();
        await playFabClient.DefaultIdLoginAsync();

        await playFabServiceMg.InitAsync(serviceRegistry, ct);

        EventManager.Instance.Trigger("UI_NextProgress");
        return StartupTaskResult.Ok();
    }
}
