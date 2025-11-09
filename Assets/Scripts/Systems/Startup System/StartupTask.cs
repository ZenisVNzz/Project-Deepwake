using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public struct StartupTaskResult
{
    public bool Success;
    public string ErrorId;     
    public string Message;

    public static StartupTaskResult Ok() =>
        new StartupTaskResult { Success = true };

    public static StartupTaskResult Fail(string errorId, string message = null) =>
        new StartupTaskResult { Success = false, ErrorId = errorId, Message = message };
}

public abstract class StartupTask : ScriptableObject, IStartupTask
{
    public abstract bool HasTimeout { get; }
    public virtual bool RequiresNetwork => false;
    public virtual bool isMainProgressTask => false;

    public abstract Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct);
}
