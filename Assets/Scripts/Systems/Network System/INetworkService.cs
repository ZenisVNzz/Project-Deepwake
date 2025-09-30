using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface INetworkService
{
    Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct);
}
