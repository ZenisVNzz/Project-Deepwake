using Facebook.Unity;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class FacebookLoginHandler : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }

        Button loginButton = GetComponent<Button>();
        loginButton.onClick.AddListener(FacebookLogin);
    }

    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

            FacebookAuth(aToken.TokenString);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void FacebookAuth(string accessToken)
    {
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
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

            var user = task.Result;
            Debug.Log($"User signed in successfully: {user.DisplayName} ({user.UserId})");

            StartupProcessor.Instance.GetService<PlayFabServiceManager>().GetService<PlayFabClient>().FacebookLoginAsync(accessToken).ContinueWith(playFabTask =>
            {
                if (playFabTask.IsFaulted || playFabTask.IsCanceled)
                {
                    Debug.LogError("PlayFab Facebook login failed: " + playFabTask.Exception);
                }
                else
                {
                    Debug.Log("PlayFab Facebook login successful!");
                }
            });
        });
    }

    public void FacebookLogin()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
}