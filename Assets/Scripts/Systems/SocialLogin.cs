using PlayFab;
using PlayFab.ClientModels;
using Facebook.Unity;
using UnityEngine;

public class SocialLogin : MonoBehaviour
{
    void Start()
    {
        if (!FB.IsInitialized) FB.Init();
    }

    public void LoginWithFacebook()
    {
        var perms = new System.Collections.Generic.List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, result => {
            if (FB.IsLoggedIn)
            {
                var accessToken = AccessToken.CurrentAccessToken.TokenString;
                var req = new LoginWithFacebookRequest
                {
                    AccessToken = accessToken,
                    CreateAccount = true
                };
                PlayFabClientAPI.LoginWithFacebook(req, OnPlayFabLoginSuccess, OnPlayFabError);
            }
            else
            {
                Debug.Log("FB login cancelled");
            }
        });
    }

    void OnPlayFabLoginSuccess(LoginResult res)
    {
        Debug.Log("PlayFab logged in: " + res.PlayFabId);
    }

    void OnPlayFabError(PlayFabError err)
    {
        Debug.LogError(err.GenerateErrorReport());
    }
}
