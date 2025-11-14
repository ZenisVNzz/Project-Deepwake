using PlayFab;
using PlayFab.ClientModels;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckUpdateTask", menuName = "StartupSystem/CheckUpdateTask")]
public class CheckUpdateTask : StartupTask
{
    public override bool HasTimeout { get { return true; } }
    public override bool RequiresNetwork => true;
    public override bool isMainProgressTask => true;

    public override async Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        Debug.Log($"[CheckUpdateTask] Check for Update.");        
        bool checkResult = await CheckGameVersionAsync();
        if (checkResult)
        {
            EventManager.Instance.Trigger("UI_NextProgress");
            return StartupTaskResult.Ok();
        }    
        return StartupTaskResult.Fail("GAME_OUTDATED", "The game version is outdated.");
    }    

    private async Task<bool> CheckGameVersionAsync()
    {
        var task = new TaskCompletionSource<bool>();

        Debug.Log($"[CheckUpdateTask] Send GetTitleData request.");
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Game_CurrentVersion"))
            {
                string serverVersion = result.Data["Game_CurrentVersion"];
                Debug.Log($"[PlayFab] Latest game version on server: {serverVersion}");
                string localVersion = Application.version;

                if (localVersion != serverVersion)
                {
                    Debug.LogWarning($"[CheckUpdateTask] Game is outdated.");
                    task.SetResult(false);
                }
                else
                {
                    Debug.Log($"[CheckUpdateTask] Game is up to date.");
                    task.SetResult(true);
                }
            }
        },
        error =>
        {
            Debug.LogError("[PlayFab] Failed to get TitleData: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return await task.Task;
    }    
}
