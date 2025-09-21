using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "NetworkMonitor", menuName = "NetworkSystem/NetworkMonitor")]
public class NetworkMonitor : NetworkService
{
    private bool _isOnline;
    private readonly string _pingUrl = "https://www.google.com";
    private readonly int _interval = 5;

    public override async Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            _isOnline = await CheckConnectionAsync();
            await Task.Delay(_interval * 1000);
        }
        Debug.Log($"[NetworkMonitor] Client status: {(_isOnline ? "Online" : "Offline")}");
        serviceRegistry.Register<NetworkMonitor>(this);
        return true;
    }

    private async Task<bool> CheckConnectionAsync()
    {
        using (var req = UnityWebRequest.Get(_pingUrl))
        {
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            return req.result == UnityWebRequest.Result.Success;
        }    
    }
}
