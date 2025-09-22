using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IPlayFabService
{
    Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct);
}
