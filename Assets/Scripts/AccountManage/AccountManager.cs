using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField emailInput;
    public InputField passwordInput;
    public Text statusText;

    private void Start()
    {
        // Guest
        if (!PlayerPrefs.HasKey("DeviceID"))
        {
            PlayerPrefs.SetString("DeviceID", SystemInfo.deviceUniqueIdentifier);
        }

        string deviceId = PlayerPrefs.GetString("DeviceID");
        LoginWithGuest(deviceId);
    }

    public void LoginWithGuest(string deviceId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = deviceId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                statusText.text = "Đăng nhập Guest thành công!";
            },
            error =>
            {
                statusText.text = "Lỗi: " + error.ErrorMessage;
            });
    }

    public void RegisterEmail()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request,
            result => { statusText.text = "Đăng ký thành công!"; },
            error => { statusText.text = "Lỗi: " + error.ErrorMessage; });
    }

    public void LoginWithEmail()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request,
            result => { statusText.text = "Đăng nhập thành công!"; },
            error => { statusText.text = "Lỗi: " + error.ErrorMessage; });
    }

    public void ResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = PlayFabSettings.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request,
            result => { statusText.text = "Đã gửi email khôi phục mật khẩu."; },
            error => { statusText.text = "Lỗi: " + error.ErrorMessage; });
    }

    public void LinkGuestToEmail()
    {
        var request = new AddUsernamePasswordRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            Username = emailInput.text.Split('@')[0]
        };

        PlayFabClientAPI.AddUsernamePassword(request,
            result => { statusText.text = "Đã liên kết tài khoản!"; },
            error => { statusText.text = "Lỗi: " + error.ErrorMessage; });
    }
}