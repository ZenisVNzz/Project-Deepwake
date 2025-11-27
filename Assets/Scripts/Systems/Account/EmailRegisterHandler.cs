using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailRegisterHandler : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;

    private void Awake()
    {
        Button registerButton = GetComponent<Button>();
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    public void OnRegisterClicked()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            Debug.LogError("All fields are required.");
            return;
        }
        if (password != confirmPassword)
        {
            Debug.LogError("Passwords do not match.");
            return;
        }

        StartupProcessor.Instance.GetService<PlayFabServiceManager>().GetService<PlayFabClient>().EmailRegesterAsync(email, password, username).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Registration failed: " + task.Exception?.GetBaseException().Message);
            }
            else
            {
                Debug.Log("Registration successful!");
            }
        });
    }
}
