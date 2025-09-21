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
    [SerializeField] private string _sceneToLoad = "Title";
    private StartupTaskList _taskList;
    private ServiceRegistry _serviceRegistry;

    private CancellationTokenSource _cts;
    [SerializeField] private float _timeout = 10f;

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

        AsyncOperationHandle<StartupTaskList> handle = Addressables.LoadAssetAsync<StartupTaskList>("StartupTaskList");
        handle.Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _taskList = (StartupTaskList)op.Result;
            }
            else
            {
                Debug.LogError("[Addressables] Load StartupTaskList failed.");
            }    
        };

        _serviceRegistry = new ServiceRegistry();
        _cts = new CancellationTokenSource();
    }

    private async void Start()
    {
        var t = await RunAllTasks(_cts.Token);
        if (t)
        {
            Debug.Log("[Startup] Startup success.");
            SceneManager.LoadScene(_sceneToLoad);
        }
        else
        {
            Debug.LogWarning("[Startup] Startup failed.");
        }
    }

    private async Task<bool> RunAllTasks(CancellationToken ct)
    {      
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
}
