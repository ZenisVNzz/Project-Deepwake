using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using UnityEngine;

public class GoogleLoginHandler : MonoBehaviour
{
    //private Action<string> onAuthCodeReceivedCallback;

    //void Start()
    //{
    //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    //        .RequestServerAuthCode(false)
    //        .RequestIdToken()
    //        .Build();

    //    PlayGamesPlatform.InitializeInstance(config);
    //    PlayGamesPlatform.Activate();

    //    Debug.Log("Google Play Games initialized.");
    //}

    //public void SignInGooglePlayGames(Action<string> callback)
    //{
    //    onAuthCodeReceivedCallback = callback;

    //    PlayGamesPlatform.Instance.Authenticate((success) =>
    //    {
    //        if (success == SignInStatus.Success)
    //        {
    //            Debug.Log("Google Play Games authenticated successfully.");

    //            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
    //            Debug.Log($"Google Server Auth Code: {authCode}");
    //            onAuthCodeReceivedCallback?.Invoke(authCode);
    //        }
    //        else
    //        {
    //            Debug.LogError($"Google Play Games authentication failed. Status: {success}");
    //            onAuthCodeReceivedCallback?.Invoke(null);
    //        }
    //    });
    //}

    //public void OnAuthCallback(SignInStatus status)
    //{
    //    if (status == SignInStatus.Success)
    //    {
    //        Debug.Log("Google Play Games authentication callback success.");

    //    }
    //    else
    //    {
    //        Debug.LogError($"Google Play Games authentication callback failed. Status: {status}");

    //    }
    //}
}