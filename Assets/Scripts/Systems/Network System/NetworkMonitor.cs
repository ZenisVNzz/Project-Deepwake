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

    public async Task<bool> CheckConnectionAsync(
    int timeoutSeconds = 3,
    int retryIntervalMs = 500,
    CancellationToken ct = default)
    {
        var startTime = Time.time;

        while (!ct.IsCancellationRequested)
        {
            using (var req = UnityWebRequest.Head(_pingUrl))
            {
                req.timeout = timeoutSeconds;
                var op = req.SendWebRequest();

                while (!op.isDone)
                {
                    if (ct.IsCancellationRequested)
                        return false;

                    await Task.Yield();
                }

                if (req.result == UnityWebRequest.Result.Success)
                    return true;
            }

            if (Time.time - startTime >= timeoutSeconds)
                return false;

            await Task.Delay(retryIntervalMs, ct);
        }

        return false;
    }
}
