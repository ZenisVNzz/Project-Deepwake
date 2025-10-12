using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour, IManager
{
    public static UIManager Instance;
    private IPopupService _popupService;
    private IFloatingTextService _singleUIService;

    public async Task<bool> InitAsync()
    {
        _popupService = new PopupService();
        _singleUIService = new FloatingTextService();
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
    public IFloatingTextService GetSingleUIService() => _singleUIService;
}
