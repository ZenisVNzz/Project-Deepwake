using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;


[CreateAssetMenu(fileName = "CheckConnectionTask", menuName = "StartupSystem/CheckConnectionTask")]

public class CheckConnectionTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        EventManager.Instance.Trigger("UI_NextProgress");
        if (serviceRegistry.TryGet<NetworkManager>(out var networkManager))
        {
            var networkMonitor = networkManager.GetService<NetworkMonitor>();
            if (networkMonitor != null)
            {
                bool isOnline = await networkMonitor.CheckConnectionAsync();
                if (isOnline)
                {
                    Debug.Log("[CheckConnectionTask] Network is online.");
                    return true;
                }
                else
                {
                    Debug.LogWarning("[CheckConnectionTask] Network is offline.");
                    UIManager.Instance.GetPopupService().Create("Popup", "NetworkError", new LocalizedString("UI", "UI_NoConnection"));
                    return false;
                }
            }    
            else
            {
                Debug.LogWarning("[CheckConnectionTask] NetworkMonitor service not found.");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("[CheckConnectionTask] NetworkManager service not found.");
            return false;
        }
    }    
}
