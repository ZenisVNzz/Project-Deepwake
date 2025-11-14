using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Deepwake.NetworkSystem;

[CreateAssetMenu(fileName = "CheckConnectionTask", menuName = "StartupSystem/CheckConnectionTask")]

public class CheckConnectionTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }
    public override bool RequiresNetwork => true;
    public override bool isMainProgressTask => true;

    public override async Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        if (serviceRegistry.TryGet<NetworkManager>(out var networkManager))
        {
            var networkMonitor = networkManager.GetService<NetworkMonitor>();
            if (networkMonitor != null)
            {
                bool isOnline;
                try
                {
                    isOnline = await networkMonitor.CheckConnectionAsync();
                }
                catch(TaskCanceledException)
                {
                    Debug.LogWarning("[CheckConnectionTask] Connection check task was canceled.");
                    return StartupTaskResult.Fail("NET_CHECK_CANCELED", "Connection check task was canceled.");
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"[CheckConnectionTask] Exception while checking connection: {ex.Message}");
                    return StartupTaskResult.Fail("NET_CHECK_EXCEPTION", "Exception while checking network connection.");
                }

                if (isOnline)
                {
                    Debug.Log("[CheckConnectionTask] Network is online.");
                    EventManager.Instance.Trigger("UI_NextProgress");
                    return StartupTaskResult.Ok();
                }
                else
                {
                    Debug.LogWarning("[CheckConnectionTask] Network is offline.");                
                    return StartupTaskResult.Fail("NET_NO_CONNECTION", "Network is offline.");
                }
            }    
            else
            {
                Debug.LogWarning("[CheckConnectionTask] NetworkMonitor service not found.");
                return StartupTaskResult.Fail("NET_NO_MONITOR", "NetworkMonitor service not found.");
            }
        }
        else
        {
            Debug.LogWarning("[CheckConnectionTask] NetworkManager service not found.");
            return StartupTaskResult.Fail("NET_NO_MANAGER", "NetworkManager service not found.");
        }
    }    
}
