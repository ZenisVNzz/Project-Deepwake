using UnityEngine;

public class EmailLoginHandler : MonoBehaviour
{
    public TMPro.TMP_InputField emailInput;
    public TMPro.TMP_InputField passwordInput;

    private void Awake()
    {
        UnityEngine.UI.Button loginButton = GetComponent<UnityEngine.UI.Button>();
        loginButton.onClick.AddListener(OnLoginClicked);
    }
    public void OnLoginClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email and password are required.");
            return;
        }
        StartupProcessor.Instance.GetService<PlayFabClient>().EmailLoginAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Login failed: " + task.Exception?.GetBaseException().Message);
            }
            else
            {
                Debug.Log("Login successful!");
            }
        });
    }
}
