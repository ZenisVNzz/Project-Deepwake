using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAuth : MonoBehaviour
{
    private Firebase.Auth.FirebaseAuth auth;

    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void SignInWithGoogle()
    {
        _ = GoogleSignIn();
    }

    private async Task GoogleSignIn()
    {
        ServiceManager.GetService<OpenIDConnectService>().LoginCompleted += OnLoginCompleted;

        await ServiceManager.GetService<OpenIDConnectService>().OpenLoginPageAsync();
    }

    private void OnLoginCompleted(object sender, EventArgs e)
    {
        string AccessToken = ServiceManager.GetService<OpenIDConnectService>().AccessToken;

        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(null, AccessToken);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
        });

        StartupProcessor.Instance.GetService<PlayFabServiceManager>().GetService<PlayFabClient>().GoogleLoginAsync(AccessToken).ContinueWith(playFabTask =>
        {
            if (playFabTask.IsFaulted || playFabTask.IsCanceled)
            {
                Debug.LogError("PlayFab Google login failed: " + playFabTask.Exception);
            }
            else
            {
                Debug.Log("PlayFab Google login successful!");
            }
        });
    }
}
