using UnityEngine; 
using Google;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.UI;

public class GoogleLoginHandler : MonoBehaviour
{
    public string GoogleWebAPI = "1042625468516-40manp0l7o04alqc71m9uqulaq28n7aj.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWebAPI,
            RequestIdToken = true
        };

        Button loginButton = GetComponent<Button>();
        loginButton.onClick.AddListener(GoogleSignInClick);
    }

    private void Start()
    {
        InitFireBase();
    }

    void InitFireBase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Google Sign-In encountered an error: " + task.Exception);
        }
        else if (task.IsCanceled)
        {
            Debug.LogWarning("Google Sign-In was canceled.");
        }
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(authTask =>
            {
                if (authTask.IsFaulted)
                {
                    Debug.LogError("Firebase authentication failed: " + authTask.Exception);
                }
                else if (authTask.IsCanceled)
                {
                    Debug.LogWarning("Firebase authentication was canceled.");
                }
                else
                {
                    user = auth.CurrentUser;
                    Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
                }
            });

            StartupProcessor.Instance.GetService<PlayFabServiceManager>().GetService<PlayFabClient>().GoogleLoginAsync(task.Result.IdToken).ContinueWith(playFabTask =>
            {
                if (playFabTask.IsFaulted || playFabTask.IsCanceled)
                {
                    Debug.LogError("PlayFab Google login failed.");
                }
                else if (playFabTask.Result)
                {
                    Debug.Log("PlayFab Google login succeeded.");
                }
            });
        }
    }
}