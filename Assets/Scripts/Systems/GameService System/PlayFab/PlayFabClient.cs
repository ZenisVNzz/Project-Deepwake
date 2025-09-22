using PlayFab;
using PlayFab.ClientModels;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayFabClient", menuName = "PlayFabService/PlayFabClient")]
public class PlayFabClient : PlayFabService
{
    public override async Task<bool> InitAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        serviceRegistry.Register<PlayFabClient>(this);
        await Task.CompletedTask;
        return true;
    }

    public async Task<bool> LoginCustomIdAsync()
    {
        var task = new TaskCompletionSource<bool>();

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("[PlayFab] PlayFab login customID method successful! PlayFabId: " + result.PlayFabId);
            task.SetResult(true);
        },
        error =>
        {
            Debug.LogError("[PlayFab] PlayFab login customID method failed: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return await task.Task;
    }
}
