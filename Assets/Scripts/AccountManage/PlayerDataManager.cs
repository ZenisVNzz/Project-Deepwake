using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SavePlayerData(Dictionary<string, string> data)
    {
        var request = new UpdateUserDataRequest
        {
            Data = data
        };

        PlayFabClientAPI.UpdateUserData(request,
            result => {
                Debug.Log("Dữ liệu đã được lưu thành công!");
            },
            error => {
                Debug.LogError("Lỗi khi lưu dữ liệu: " + error.GenerateErrorReport());
            }
        );
    }

    public void LoadPlayerData(Action<Dictionary<string, UserDataRecord>> onDataLoaded)
    {
        var request = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(request,
            result => {
                Debug.Log("Dữ liệu đã được tải.");
                onDataLoaded?.Invoke(result.Data);
            },
            error => {
                Debug.LogError("Lỗi khi tải dữ liệu: " + error.GenerateErrorReport());
                onDataLoaded?.Invoke(null);
            }
        );
    }

    // Test đăng nhập và lưu dữ liệu
    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Đăng nhập thành công! ID: " + result.PlayFabId);

        Dictionary<string, string> data = new Dictionary<string, string>()
    {
        {"Level", "5"},
        {"Gold", "1500"},
        {"Character", "HeroKnight"}
    };

        PlayerDataManager.Instance.SavePlayerData(data);
    }

    // Test tải dữ liệu
    void Start()
    {
        PlayerDataManager.Instance.LoadPlayerData(data =>
        {
            if (data != null && data.ContainsKey("Gold"))
            {
                Debug.Log("Người chơi có: " + data["Gold"].Value + " vàng.");
            }
            else
            {
                Debug.Log("Chưa có dữ liệu — tạo mới nếu cần.");
            }
        });
    }
}