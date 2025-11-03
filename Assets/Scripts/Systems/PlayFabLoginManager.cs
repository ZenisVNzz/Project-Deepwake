using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic; 

public class PlayFabLoginManager : MonoBehaviour
{
    public Text statusText; 
    public Button googleLoginButton;
    public Button facebookLoginButton;

    private GoogleLoginHandler googleLoginHandler;
    private FacebookLoginHandler facebookLoginHandler;

    void Awake()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "17FA31";
        }
    }

    void Start()
    {
        statusText.text = "Ready to Login";

        googleLoginHandler = FindObjectOfType<GoogleLoginHandler>();
        facebookLoginHandler = FindObjectOfType<FacebookLoginHandler>();

        googleLoginButton.onClick.AddListener(OnGoogleLoginButtonClicked);
        facebookLoginButton.onClick.AddListener(OnFacebookLoginButtonClicked);

        LoginAnonymously();
    }

    void LoginAnonymously()
    {
        statusText.text = "Logging in anonymously...";
#if UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceID = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
#elif UNITY_IOS
        var request = new LoginWithIOSDeviceIDRequest
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSuccess, OnLoginFailure);
#else
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
#endif
    }

    void OnGoogleLoginButtonClicked()
    {
        statusText.text = "Initiating Google Login...";
        if (googleLoginHandler != null)
        {
            googleLoginHandler.SignInGooglePlayGames(OnGoogleAuthCodeReceived);
        }
        else
        {
            statusText.text = "GoogleLoginHandler not found!";
        }
    }

    void OnFacebookLoginButtonClicked()
    {
        statusText.text = "Initiating Facebook Login...";
        if (facebookLoginHandler != null)
        {
            facebookLoginHandler.SignInFacebook(OnFacebookAccessTokenReceived);
        }
        else
        {
            statusText.text = "FacebookLoginHandler not found!";
        }
    }

    public void OnGoogleAuthCodeReceived(string serverAuthCode)
    {
        if (!string.IsNullOrEmpty(serverAuthCode))
        {
            statusText.text = "Google Auth Code received. Logging in with PlayFab...";
            LoginWithPlayFabGoogle(serverAuthCode);
        }
        else
        {
            statusText.text = "Google Login failed: No Auth Code.";
        }
    }

    public void OnFacebookAccessTokenReceived(string accessToken)
    {
        if (!string.IsNullOrEmpty(accessToken))
        {
            statusText.text = "Facebook Access Token received. Logging in with PlayFab...";
            LoginWithPlayFabFacebook(accessToken);
        }
        else
        {
            statusText.text = "Facebook Login failed: No Access Token.";
        }
    }

    void LoginWithPlayFabGoogle(string serverAuthCode)
    {
        var request = new LoginWithGoogleAccountRequest
        {
            ServerAuthCode = serverAuthCode,
            CreateAccount = true, 
            TitleId = PlayFabSettings.TitleId
        };
        PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoginSuccess, OnLoginFailure);
    }

    void LoginWithPlayFabFacebook(string accessToken)
    {
        var request = new LoginWithFacebookRequest
        {
            AccessToken = accessToken,
            CreateAccount = true, 
            TitleId = PlayFabSettings.TitleId 
        };
        PlayFabClientAPI.LoginWithFacebook(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        statusText.text = $"Login Successful! PlayFab ID: {result.PlayFabId}";
        Debug.Log($"PlayFab User ID: {result.PlayFabId}");

        if (result.InfoResultPayload != null && result.InfoResultPayload.PlayerProfile != null)
        {
            statusText.text += $"\nWelcome, {result.InfoResultPayload.PlayerProfile.DisplayName}!";
        }
        else if (result.NewlyCreated)
        {
            statusText.text += "\nNew PlayFab account created!";
        }
    }

    void OnLoginFailure(PlayFabError error)
    {
        statusText.text = $"Login Failed: {error.ErrorMessage}";
        Debug.LogError($"PlayFab Login Error: {error.GenerateErrorReport()}");
    }

    void SetDefaultDisplayName(string defaultName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = defaultName
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => Debug.Log("Display name updated!"),
            error => Debug.LogError($"Error setting display name: {error.GenerateErrorReport()}")
        );
    }
}