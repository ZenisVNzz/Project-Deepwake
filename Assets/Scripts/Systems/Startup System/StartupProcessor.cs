using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class StartupProcessor : MonoBehaviour
{
    public static StartupProcessor Instance;
    private StartupTaskList _taskList;
    private IServiceRegistry _serviceRegistry;

    private CancellationTokenSource _cts;
    [SerializeField] private float _timeout = 10f;

    private InputSystem_Actions inputActions;
    private bool isCompleted = false;
    private bool isLoadingScene = false;

    private async void Awake()
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

        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += ctx => WaitForClick();

        AsyncOperationHandle<StartupTaskList> handle = Addressables.LoadAssetAsync<StartupTaskList>("StartupTaskList");
        _taskList = await handle.Task;

        _serviceRegistry = new ServiceRegistry();
        _cts = new CancellationTokenSource();

        var t = await RunAllTasks(_cts.Token);
        if (t)
        {
            Debug.Log("[Startup] Startup success.");
            ResourceManager.Instance.ReleaseAssetReferences("Startup");
            EventManager.Instance.Trigger("UI_NextProgress");
            isCompleted = true;
        }
        else
        {
            Debug.LogWarning("[Startup] Startup failed.");
        }
    }

    private async Task<bool> RunAllTasks(CancellationToken ct)
    {
        if (_taskList.TaskList.Count <= 0)
        {
            Debug.LogWarning("[Startup] No task to do.");
            return false;
        }

        foreach (var task in _taskList.TaskList)
        {
            string TaskName = task.GetType().Name;
            Debug.Log($"[Startup] Running task: {TaskName}");
            using (var timeoutCTS = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                timeoutCTS.CancelAfter(TimeSpan.FromSeconds(_timeout));
                try
                {
                    var t = task.HasTimeout ? await task.RunTaskAsync(_serviceRegistry, CancellationToken.None) : await task.RunTaskAsync(_serviceRegistry, timeoutCTS.Token);
                    if (!t)
                    {
                        Debug.LogWarning($"[Startup] Task failed: {TaskName}");
                        return false;
                    }
                    else
                    {
                        Debug.Log($"[Startup] Task successfully: {TaskName}");
                    }    
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning($"[Startup] Task timeout or cancelled: {TaskName}");
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex); 
                    return false;                  
                }
            }    
        }   
        return true;
    }  

    private async void WaitForClick()
    {   
        if (isCompleted)
        {
            if (!isLoadingScene)
            {
                isLoadingScene = true;
                await SceneLoader.Instance.LoadScene("Loading");
                inputActions.UI.Disable();
            }      
        }     
    }
    
    public Tservice GetService<Tservice>()
    {
        return _serviceRegistry.Get<Tservice>();
    }    

    public bool GetService<Tservice>(out Tservice service)
    {
        return _serviceRegistry.TryGet<Tservice>(out service);
    }    
}
