using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabAuth : MonoBehaviour
{
    public void LoginWithEmail(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetUserAccountInfo = true }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Đăng nhập thành công! PlayFab ID: " + result.PlayFabId);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Đăng nhập thất bại: " + error.GenerateErrorReport());
    }

    public void LoginWithGoogle(string serverAuthCode)
    {
        var request = new LoginWithGoogleAccountRequest
        {
            ServerAuthCode = serverAuthCode,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoginSuccess, OnLoginFailure);
    }

        public void LoginWithFacebook(string fbAccessToken)
    {
        var request = new LoginWithFacebookRequest
        {
            AccessToken = fbAccessToken,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithFacebook(request, OnLoginSuccess, OnLoginFailure);
    }
    public void LoginAsGuest()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }


}

