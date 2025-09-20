using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IGameService
{
    Task InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct);
}
