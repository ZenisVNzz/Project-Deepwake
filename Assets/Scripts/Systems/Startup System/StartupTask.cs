using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StartupTask : ScriptableObject, IStartupTask
{
    public abstract bool HasTimeout { get; }
    public abstract Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct);
}
