using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IGameService
{
    Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct);
}
