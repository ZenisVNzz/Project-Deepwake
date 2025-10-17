//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Firebase.Extensions;
//using Google;
//using System.Threading.Tasks;
//using TMPro;
//using Firebase.Auth;
//using UnityEngine.UI;

//public class GoogleLogin1 : MonoBehaviour
//{
//    public string GoogleAPI = "Enter web client id here";
//    private GoogleSignInConfiguration cofiguration;

//    Firebase.Auth.FirebaseAuth auth;
//    Firebase.Auth.FirebaseUser user;
    
//    public TextMeshProUGUI Username, UserEmail;
    
//    public Image UserProfilePic;
//    private string imageUrl;
//    private bool isGoogleSignInInitialized = false;
//    private void Start()
//    {
//        InitFirebase();
//    }

//    void InitFirebase()
//    {
//        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
//    }

//    public void Login()
//    {
//        if(!isGoogleSignInInitialized)
//        {
//            GoogleSignIn.Configuration = new GoogleSignInConfiguration
//            {
//                RequestIdToken = true,
//                WebClientId = GoogleAPI,
//                RequestEmail = true
//            };

//            isGoogleSignInInitialized=true;
//        }
//        isGoogleSignInInitialized.Configuration = new GoogleSignInConfiguration
//        {
//            RequestIdToken = true,
//            WebClientId = GoogleAPI,
//        };
//        GoogleSignIn.Configuration.RequestEmail = true;

//        Task<GoogleSignInUser> signIn = isGoogleSignInInitialized.DefaultInstance.SignIn();

//        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
//        signIn.ContinueWith(task =>
//        {
//            if (task.IsCanceled)
//            {
//                signInCompleted.SetCanceled();
//                Debug.Log("Cancelled");
//            }
//            else if (task.IsFaulted)
//            { 
//                signInCompleted.SetException(task.Exception);

//                Debug.Log("Faulted" + task.Exception);
//            }
//            else
//            {
//                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken).auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
//                {

//                })
//            }
//        })
//    }

//}
