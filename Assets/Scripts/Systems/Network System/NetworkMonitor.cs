using System;
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

    private CancellationTokenSource _cts;

    public override async Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        _cts = new CancellationTokenSource();
        _isOnline = await CheckConnectionAsync();
        Debug.Log($"[NetworkMonitor] Client status: {(_isOnline ? "Online" : "Offline")}");
        serviceRegistry.Register<NetworkMonitor>(this);
        _ = MonitorLoop(_cts.Token);
        return true;
    }

    private async Task MonitorLoop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            bool IsOnline = await CheckConnectionAsync();
            if (_isOnline && !IsOnline)
            {
                Debug.LogWarning("[NetworkMonitor] Connection Lost.");
            }    
            _isOnline = IsOnline;
            await Task.Delay(_interval * 1000);
        }
    }

    public async Task<bool> CheckConnectionAsync()
    {
        using (var req = UnityWebRequest.Get(_pingUrl))
        using (var timeoutCTS = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
        {
            var op = req.SendWebRequest();

            while (!op.isDone)
            {
                if (timeoutCTS.Token.IsCancellationRequested)
                {
                    req.Abort();
                    return false;
                }
                await Task.Yield();
            }

            return req.result == UnityWebRequest.Result.Success;
        }
    }
}
