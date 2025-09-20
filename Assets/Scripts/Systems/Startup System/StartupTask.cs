using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StartupTask : ScriptableObject, IStartupTask
{
    public abstract Task<bool> RunTask(IServiceRegistry serviceRegistry, CancellationToken ct);
}
