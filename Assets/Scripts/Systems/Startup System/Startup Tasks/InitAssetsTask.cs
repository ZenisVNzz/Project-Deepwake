using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "InitAssetsTask", menuName = "StartupSystem/InitAssetsTask")]
public class InitAssetsTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }

    public override async Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        ResourceManager resourceManager = ResourceManager.Instance;
        try
        {
            await resourceManager.Preload("Startup");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[InitAssetsTask] Exception while preloading assets: {ex.Message}");
            return StartupTaskResult.Fail("ASSET_PRELOAD_EXCEPTION", "Exception while preloading assets.");
        }

        return StartupTaskResult.Ok();
    }    
}
