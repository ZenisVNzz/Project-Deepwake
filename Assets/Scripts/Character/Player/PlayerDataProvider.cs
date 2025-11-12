using TMPro;
using UnityEngine;

public class PlayerDataProvider : MonoBehaviour
{
    public static PlayerDataProvider Instance;
    public TMP_InputField playerNameInputField;

    public string playerName = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerNameInputField.onValueChanged.AddListener(OnPlayerNameChanged);
    }

    public void OnPlayerNameChanged(string newName)
    {
        playerName = newName;
    }
}
