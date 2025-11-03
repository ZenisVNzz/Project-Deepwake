using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

 void LoginWithGoogle(string idTokenOrServerAuthCode)
{
    var req = new LoginWithGoogleAccountRequest
    {
        ServerAuthCode = idTokenOrServerAuthCode,
        CreateAccount = true
    };
    PlayFabClientAPI.LoginWithGoogleAccount(req, OnLoginSuccess, OnLoginFailure);
}
