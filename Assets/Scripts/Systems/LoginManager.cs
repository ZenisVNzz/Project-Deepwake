using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField emailInput;
    public InputField passwordInput;
    public Text messageText;

    private void Start()
    {
        messageText.text = "";
    }

    public void OnLoginButton()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Vui lòng nhập email và mật khẩu.";
            return;
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void OnRegisterButton()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Vui lòng nhập email và mật khẩu.";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void OnGuestButton()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }


    private void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Đăng nhập thành công!";
        Debug.Log("Login Success: " + result.PlayFabId);
        SceneManager.LoadScene("LobbyScene"); // Chuyển sang scene tiếp theo
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Tạo tài khoản thành công!";
        Debug.Log("Register Success: " + result.PlayFabId);
    }

    private void OnError(PlayFabError error)
    {
        messageText.text = "Lỗi: " + error.ErrorMessage;
        Debug.LogError(error.GenerateErrorReport());
    }
}
