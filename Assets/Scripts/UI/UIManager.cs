using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour, IManager
{
    public static UIManager Instance;
    private IPopupService _popupService;

    public async Task<bool> InitAsync()
    {
        _popupService = new PopupService();
        await Task.CompletedTask;
        return true;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public IPopupService GetPopupService() => _popupService;
}
