using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PlayFabLoginManager : MonoBehaviour
{
    public Text statusText;

    public Button googleLoginButton;
    public Button facebookLoginButton;
    public Button emailLoginButton;
    public Button emailRegisterButton;

    public TMPro.TMP_InputField loginEmailInput;
    public TMPro.TMP_InputField loginPasswordInput;
    public TMPro.TMP_InputField registerUsernameInput;
    public TMPro.TMP_InputField registerEmailInput;
    public TMPro.TMP_InputField registerPasswordInput;
    public TMPro.TMP_InputField registerConfirmPasswordInput;


    [SerializeField]
    private PlayerData playerData = new PlayerData();

    public CurrencyWallet CurrencyWallet = new CurrencyWallet();

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
        emailLoginButton?.onClick.AddListener(OnEmailLoginClicked);
        emailRegisterButton?.onClick.AddListener(OnEmailRegisterClicked);

        LoginAnonymously();
    }

    void LoginAnonymously()
    {
        statusText.text = "Logging in anonymously...";

#if UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
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

    public void OnEmailLoginClicked()
    {
        string email = loginEmailInput.text;
        string password = loginPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Email and password are required.";
            return;
        }

        statusText.text = "Attempting Email Login...";

        StartupProcessor.Instance.GetService<PlayFabServiceManager>().GetService<PlayFabClient>().EmailLoginAsync(email, password).ContinueWith(task =>
        {

            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                if (task.IsFaulted)
                {
                    string errorMsg = "Login failed: " + task.Exception?.GetBaseException().Message;
                    Debug.LogError(errorMsg);
                    statusText.text = errorMsg;
                }
                else
                {
                    statusText.text = "Login successful! Loading data...";
                    GetPlayerData();
                }
            });
        });
    }

    public void OnEmailRegisterClicked()
    {
        string username = registerUsernameInput.text;
        string email = registerEmailInput.text;
        string password = registerPasswordInput.text;
        string confirmPassword = registerConfirmPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            statusText.text = "All fields are required for registration.";
            return;
        }
        if (password != confirmPassword)
        {
            statusText.text = "Passwords do not match.";
            return;
        }

        statusText.text = "Attempting Email Registration...";

        StartupProcessor.Instance.GetService<PlayFabServiceManager>().GetService<PlayFabClient>().EmailRegesterAsync(email, password, username).ContinueWith(task =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                if (task.IsFaulted)
                {
                    string errorMsg = "Registration failed: " + task.Exception?.GetBaseException().Message;
                    Debug.LogError(errorMsg);
                    statusText.text = errorMsg;
                }
                else
                {
                    Debug.Log("Registration successful! You can now log in.");
                    statusText.text = "Registration successful! Please log in.";
                }
            });
        });
    }

    void OnLoginSuccess(LoginResult result)
    {
        statusText.text = $"Login Successful! PlayFab ID: {result.PlayFabId}";
        GetPlayerData();

        if (result.NewlyCreated)
        {
            statusText.text += "\nNew PlayFab account created!";
            SetPlayerDisplayName("NewPlayer");
        }
    }

    void OnLoginFailure(PlayFabError error)
    {
        string errorMsg = (error != null) ? error.ErrorMessage : "Login operation failed.";
        statusText.text = $"Login Failed: {errorMsg}";
        Debug.LogError($"PlayFab Login Error: {(error != null ? error.GenerateErrorReport() : errorMsg)}");
    }

    void GetPlayerData()
    {
        statusText.text = "Fetching Player Data...";
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnDataError);
    }

    void OnDataReceived(GetUserDataResult result)
    {
        statusText.text = "Player Data Loaded!";

        const string KEY = PlayerData.CHARACTER_STATE_KEY;

        if (result.Data != null && result.Data.ContainsKey(KEY))
        {
            string json = result.Data[KEY].Value;
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.Log("No existing data found. Using default PlayerData.");
        }

        SyncWalletFromPlayerData();

        statusText.text += $"\nLevel: {playerData.PlayerLevel} | Gold: {CurrencyWallet.Gold} | Last Login: {playerData.LastLoginTime}";
    }

    void SyncWalletFromPlayerData()
    {
        if (playerData.CurrencyWallet.Balances != null)
        {
            foreach (var entry in playerData.CurrencyWallet.Balances)
            {
                if (Enum.TryParse(entry.Key, out CurrencyType type))
                {
                    CurrencyWallet.Set(type, entry.Value, false);
                }
            }
        }
    }


    public void SavePlayerData()
    {
        playerData.CurrencyWallet = new PlayerCurrencyState(CurrencyWallet);
        playerData.PlayerLevel += 1;

        string jsonToSave = JsonUtility.ToJson(playerData);

        statusText.text = "Saving Player Data...";

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { PlayerData.CHARACTER_STATE_KEY, jsonToSave }
            },
            Permission = UserDataPermission.Private
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSaved, OnDataError);
    }

    void OnDataSaved(UpdateUserDataResult result)
    {
        statusText.text = $"Data Saved Successfully! New Level: {playerData.PlayerLevel}";
    }


    void OnDataError(PlayFabError error)
    {
        statusText.text = $"Data Operation Failed: {error.ErrorMessage}";
        Debug.LogError($"PlayFab Data Error: {error.GenerateErrorReport()}");
    }

    void SetPlayerDisplayName(string defaultName)
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = defaultName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => Debug.Log("Display name updated!"),
            OnDataError
        );
    }
}