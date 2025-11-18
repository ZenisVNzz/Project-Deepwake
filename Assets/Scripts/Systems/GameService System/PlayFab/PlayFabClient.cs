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

    public async Task<bool> DefaultIdLoginAsync()
    {
        var task = new TaskCompletionSource<bool>();

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "70844766C918E6C2",
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("[PlayFab] PlayFab customID login successful! PlayFabId: " + result.PlayFabId);
            task.SetResult(true);
        },
        error =>
        {
            Debug.LogError("[PlayFab] PlayFab customID login failed: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return await task.Task;
    }

    public Task<bool> GoogleLoginAsync(string Token)
    {
        var task = new TaskCompletionSource<bool>();

        var request = new LoginWithGoogleAccountRequest
        {
            TitleId = PlayFabSettings.TitleId,
            ServerAuthCode = Token,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithGoogleAccount(request, result =>
        {
            Debug.Log("[PlayFab] PlayFab google login success! ID: " + result.PlayFabId);
            task.SetResult(true);
        },
        error =>
        {
            Debug.LogError("[PlayFab] PlayFab google login failed: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return task.Task;
    }

    public Task<bool> FacebookLoginAsync(string Token)
    {
        var task = new TaskCompletionSource<bool>();

        var request = new LoginWithFacebookRequest
        {
            TitleId = PlayFabSettings.TitleId,
            AccessToken = Token,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithFacebook(request, result =>
        {
            Debug.Log("[PlayFab] PlayFab facebook login success! ID: " + result.PlayFabId);
            task.SetResult(true);
        },
        error =>
        {
            Debug.LogError("[PlayFab] PlayFab facebook login failed: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return task.Task;
    }  
    
    public async Task<bool> EmailRegesterAsync(string email, string password, string username)
    {
        var task = new TaskCompletionSource<bool>();

        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            Username = username,
            RequireBothUsernameAndEmail = true
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, result =>
        {
            Debug.Log("[PlayFab] PlayFab email register success! ID: " + result.PlayFabId);
            task.SetResult(true);
        },
        error =>
        {
            Debug.LogError("[PlayFab] PlayFab email register failed: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return await task.Task;
    }

    public async Task<bool> EmailLoginAsync(string email, string password)
    {
        var task = new TaskCompletionSource<bool>();

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, result =>
        {
            Debug.Log("[PlayFab] PlayFab email login success! ID: " + result.PlayFabId);
            task.SetResult(true);
        },
        error =>
        {
            Debug.LogError("[PlayFab] PlayFab email login failed: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return await task.Task;
    }

    public async Task<bool> RecoveryPassword(string email)
    {
        var task = new TaskCompletionSource<bool>();

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = PlayFabSettings.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, result =>
        {
            Debug.Log("[PlayFab] Recovery email sent");
            task.SetResult(true);
        },
        error =>
        {
            Debug.LogError("[PlayFab] Failed to send recovery email: " + error.GenerateErrorReport());
            task.SetResult(false);
        });

        return await task.Task;
    }
}
