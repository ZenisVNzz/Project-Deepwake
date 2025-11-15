using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic; // For List

public class FacebookLoginHandler : MonoBehaviour
{
    private System.Action<string> onAccessTokenReceivedCallback;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(OnInitComplete, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void OnInitComplete()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Facebook SDK initialized successfully.");
        }
        else
        {
            Debug.LogError("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SignInFacebook(System.Action<string> callback)
    {
        onAccessTokenReceivedCallback = callback;

        if (FB.IsLoggedIn)
        {
            AccessToken aToken = AccessToken.CurrentAccessToken;
            Debug.Log($"Facebook already logged in. Access Token: {aToken.TokenString}");
            onAccessTokenReceivedCallback?.Invoke(aToken.TokenString);
            return;
        }

        var permissions = new List<string>() { "public_profile", "email" };

        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            AccessToken aToken = AccessToken.CurrentAccessToken;
            Debug.Log($"Facebook Login successful. Access Token: {aToken.TokenString}");
            onAccessTokenReceivedCallback?.Invoke(aToken.TokenString);
        }
        else
        {
            Debug.LogError($"Facebook Login Failed. Error: {result.Error}");
            onAccessTokenReceivedCallback?.Invoke(null);
        }
    }
}