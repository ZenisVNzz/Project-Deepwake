using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "InitAssetsTask", menuName = "StartupSystem/InitAssetsTask")]
public class InitAssetsTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        ResourceManager resourceManager = ResourceManager.Instance;
        await resourceManager.Preload("Startup");
        return true;
    }    
}
