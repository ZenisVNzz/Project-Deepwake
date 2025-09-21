using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "InitNetworkTask", menuName = "StartupSystem/InitNetworkTask")]
public class InitNetworkTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        var Net = new NetworkManager();
        await Net.InitAsync(serviceRegistry, ct);

        return true;
    }
}
