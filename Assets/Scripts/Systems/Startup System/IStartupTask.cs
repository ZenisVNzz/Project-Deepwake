using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IStartupTask
{
    Task<bool> RunTask(IServiceRegistry serviceRegistry, CancellationToken ct);
}
