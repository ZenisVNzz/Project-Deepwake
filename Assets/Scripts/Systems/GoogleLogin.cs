//using UnityEngine;
//using Firebase;
//using Firebase.Auth;
//using System.Threading.Tasks;
//using JetBrains.Annotations;

//public class GoogleLogin : MonoBehaviour
//{
//    private FirebaseAuth auth;
//    private FirebaseUser user;

//    void Start()
//    {
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//        {
//            if (task.Result == DependencyStatus.Available)
//            {
//                auth = FirebaseAuth.DefaultInstance;
//                Debug.Log("Firebase Auth initialized");
//            }
//            else
//            {
//                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
//            }
//        });
//    }

//    public void SignWithGoogle(string idToken, string accessToken)
//    {
//        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
//        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
//        {
//            if (task.IsCanceled)
//            {
//                Debug.LogError("SignInWithCredentialAsync was canceled");
//                return;
//            }
//            if (task.IsFaulted)
//            {
//                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
//                return;
//            }
//            user = task.Result;
//            Debug.LogFormat("User signed in successfully: {0} {1}", user.DisplayName, user.Email);
//        });
//    }
//    public void SignOut()
//    {
//        auth.SignOut();
//        Debug.Log("User signed out");
//    }
//}
