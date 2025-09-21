using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IStartupTask
{
    bool HasTimeout { get; }
    Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct);
}
