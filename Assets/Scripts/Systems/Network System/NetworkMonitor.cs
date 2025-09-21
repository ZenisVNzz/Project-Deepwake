using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "NetworkMonitor", menuName = "NetworkSystem/NetworkMonitor")]
public class NetworkMonitor : NetworkService
{
    private bool _isOnline;
    public bool IsOnline => _isOnline;
    private readonly string _pingUrl = "https://www.google.com";
    private readonly int _interval = 5;

    public override async Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        _isOnline = await CheckConnectionAsync();
        Debug.Log($"[NetworkMonitor] Client status: {(_isOnline ? "Online" : "Offline")}");
        serviceRegistry.Register<NetworkMonitor>(this);
        _ = MonitorLoop(ct);
        return true;
    }

    private async Task MonitorLoop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            _isOnline = await CheckConnectionAsync();
            await Task.Delay(_interval * 1000, ct);
        }
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
