using System;
using System.Threading.Tasks;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Facebook.Unity;
using Google; // depends on Google Sign-In Unity plugin

public class LoginManager : MonoBehaviour
{
    [Header("PlayFab")]
    [Tooltip("PlayFab TitleId (leave blank to use PlayFabSettings.TitleId)")]
    public string playFabTitleId = "";

    [Header("Google Sign-In")]
    [Tooltip("Web client ID from Google Cloud Console. Must be a Web OAuth client and RequestIdToken must be true.")]
    public string googleWebClientId = "https://console.cloud.google.com/welcome?project=gameproject-472918";

    [Header("Facebook")]
    [Tooltip("Optional: you'll still need FB App configured in Facebook Developer and FB SDK initialized.")]
    public string facebookAppId = "YOUR_FACEBOOK_APP_ID";

    void Awake()
    {
        // Set PlayFab TitleId if provided
        if (!string.IsNullOrEmpty(playFabTitleId))
            PlayFab.PlayFabSettings.TitleId = playFabTitleId;

        // Init Facebook SDK
        if (!FB.IsInitialized)
        {
            FB.Init(() => {
                Debug.Log("FB initialized: " + FB.IsInitialized);
                if (FB.IsInitialized) FB.ActivateApp();
            }, isGameShown => {
                // pause/unpause handling
                Time.timeScale = isGameShown ? 1 : 0;
            });
        }

        // Configure Google Sign-In
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            WebClientId = googleWebClientId,
            RequestIdToken = true
        };

        // Optional: silent sign-in attempt on start
        // GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnGoogleSignInComplete);
    }

    #region Facebook Login
    public void LoginWithFacebook()
    {
        var perms = new System.Collections.Generic.List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, OnFacebookAuthCallback);
    }

    private void OnFacebookAuthCallback(ILoginResult result)
    {
        if (result == null) { Debug.LogError("FB login result null"); return; }
        if (FB.IsLoggedIn)
        {
            var accessToken = AccessToken.CurrentAccessToken.TokenString;
            Debug.Log("FB Access Token obtained");
            var request = new LoginWithFacebookRequest
            {
                AccessToken = accessToken,
                CreateAccount = true
            };
            PlayFabClientAPI.LoginWithFacebook(request, OnPlayFabFacebookLoginSuccess, OnPlayFabError);
        }
        else
        {
            Debug.Log("FB login canceled or failed: " + result.Error);
        }
    }

    private void OnPlayFabFacebookLoginSuccess(LoginResult res)
    {
        Debug.Log("PlayFab logged in via Facebook. PlayFabId: " + res.PlayFabId);
        // TODO: load player data, call your post-login flows
    }
    #endregion

    #region Google Login (idToken flow)
    public void LoginWithGoogle()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            WebClientId = googleWebClientId,
            RequestIdToken = true
        };

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleSignInComplete);
    }

    // Callback for Google Sign-In Task
    private void OnGoogleSignInComplete(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Google Sign-In faulted: " + task.Exception);
            return;
        }
        if (task.IsCanceled)
        {
            Debug.LogWarning("Google Sign-In canceled");
            return;
        }

        GoogleSignInUser user = task.Result;
        if (user == null)
        {
            Debug.LogError("Google user is null after sign-in");
            return;
        }

        string idToken = user.IdToken;
        if (string.IsNullOrEmpty(idToken))
        {
            Debug.LogError("No idToken returned from Google Sign-In. Make sure RequestIdToken = true and WebClientId is correct.");
            return;
        }

        Debug.Log("Google idToken obtained. Sending to PlayFab...");
        var request = new LoginWithGoogleAccountRequest
        {
            IdToken = idToken,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithGoogleAccount(request, OnPlayFabGoogleLoginSuccess, OnPlayFabError);
    }

    private void OnPlayFabGoogleLoginSuccess(LoginResult res)
    {
        Debug.Log("PlayFab logged in via Google. PlayFabId: " + res.PlayFabId);
        // TODO: load player data, call your post-login flows
    }
    #endregion

    #region PlayFab Generic
    private void OnPlayFabError(PlayFabError err)
    {
        Debug.LogError("PlayFab Error: " + err.GenerateErrorReport());
        // You can handle specific code paths here (e.g., account already exists, link required...)
    }

    // Example: link Facebook account to current PlayFab account (after PlayFab login with Google/guest)
    public void LinkFacebookToPlayFab()
    {
        if (!FB.IsLoggedIn) { Debug.LogWarning("FB not logged in"); return; }
        var token = AccessToken.CurrentAccessToken.TokenString;
        var req = new LinkFacebookAccountRequest { AccessToken = token };
        PlayFabClientAPI.LinkFacebookAccount(req, result => Debug.Log("Linked FB account"), OnPlayFabError);
    }

    public void LinkGoogleToPlayFabUsingIdToken(string idToken)
    {
        var req = new LinkGoogleAccountRequest { IdToken = idToken };
        PlayFabClientAPI.LinkGoogleAccount(req, result => Debug.Log("Linked Google account"), OnPlayFabError);
    }
    #endregion

    #region Optional: Guest / Anonymous Login
    public void LoginAsGuest(string customId = null)
    {
        var id = string.IsNullOrEmpty(customId) ? SystemInfo.deviceUniqueIdentifier : customId;
        var req = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(req, result => Debug.Log("PlayFab Guest logged in: " + result.PlayFabId), OnPlayFabError);
    }
    #endregion

    #region Logout
    public void LogoutFacebook()
    {
        if (FB.IsLoggedIn) FB.LogOut();
    }

    // Google Sign-Out
    public void LogoutGoogle()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }
    #endregion

    // Small helper to show user-friendly messages (customize or tie into UI)
    private void ShowMessage(string text)
    {
        Debug.Log(text);
        // TODO: wire to your UI toast / popup
    }
}
