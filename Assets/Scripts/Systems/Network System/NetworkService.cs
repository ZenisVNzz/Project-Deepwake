using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NetworkService : ScriptableObject, INetworkService
{
    public abstract Task<bool> InitAsync(IServiceRegistry serviceRegistry ,CancellationToken ct);
}
