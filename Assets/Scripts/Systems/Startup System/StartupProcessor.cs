using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    private bool _offlineMode = false;

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
            return;
        }

        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += ctx => WaitForClick();

        AsyncOperationHandle<StartupTaskList> handle = Addressables.LoadAssetAsync<StartupTaskList>("StartupTaskList");
        _taskList = await handle.Task;

        _serviceRegistry = new ServiceRegistry();
        _cts = new CancellationTokenSource();

        var pipelineResult = await RunAllTasks(_cts.Token);
        if (pipelineResult.Success)
        {
            Debug.Log("[Startup] Startup success.");
            ResourceManager.Instance.ReleaseAssetReferences("Startup");
            if (_offlineMode)
            {
                EventManager.Instance.Trigger("UI_NextProgress");
            }

            isCompleted = true;
        }
        else
        {
            Debug.LogWarning($"[Startup] Startup failed. ErrorId={pipelineResult.ErrorId} Message={pipelineResult.Message}");
            var errorHandler = new StartupErrorHandler();
            errorHandler.ThrowError(pipelineResult.ErrorId);
        }
    }

    private struct StartupPipelineResult
    {
        public bool Success;
        public string ErrorId;
        public string Message;
        public static StartupPipelineResult Ok() => new StartupPipelineResult { Success = true };
        public static StartupPipelineResult Fail(string id, string msg) => new StartupPipelineResult { Success = false, ErrorId = id, Message = msg };
    }

    private async Task<StartupPipelineResult> RunAllTasks(CancellationToken ct)
    {
        if (_taskList.TaskList.Count <= 0)
        {
            Debug.LogWarning("[Startup] No task to do.");
            return StartupPipelineResult.Fail("NO_TASKS", "No startup tasks configured.");
        }

        int index = 0;
        while (index < _taskList.TaskList.Count)
        {
            var task = _taskList.TaskList[index];
            string taskName = task.GetType().Name;

            if (_offlineMode && task.RequiresNetwork)
            {
                Debug.Log($"[Startup] Skipping network task in offline mode: {taskName}");
                if (task.isMainProgressTask)
                {
                    EventManager.Instance.Trigger("UI_NextProgress");
                }

                index++;
                continue;
            }

            Debug.Log($"[Startup] Running task: {taskName}");
            using (var timeoutCTS = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                timeoutCTS.CancelAfter(TimeSpan.FromSeconds(_timeout));
                try
                {
                    CancellationToken effectiveCt = task.HasTimeout ? CancellationToken.None : timeoutCTS.Token;
                    var result = await task.RunTaskAsync(_serviceRegistry, effectiveCt);

                    if (!result.Success)
                    {
                        Debug.LogWarning($"[Startup] Task failed: {taskName} ErrorId={result.ErrorId} Message={result.Message}");

                        if (result.ErrorId == "NET_NO_CONNECTION")
                        {
                            UIManager.Instance.GetPopupService().Create("100002", "NetworkError", new LocalizedString("UI", "UI_NetworkError"), OnNetworkRetryClicked, OnContinueClicked);
                            await WaitForNetworkDecision();
                            if (isContinueClicked)
                            {
                                isContinueClicked = false;
                                _offlineMode = true;
                                index++;
                                continue;
                            }
                            else if (isRetryClicked)
                            {
                                isRetryClicked = false;
                                continue;
                            }
                        }
                        else if (result.ErrorId == "GAME_OUTDATED")
                        {
                            UIManager.Instance.GetPopupService().Create("100001", "GAME_OUTDATED", new LocalizedString("UI", "UI_GAME_OUTDATED"), OnContinueClicked, null);
                            await WaitForNetworkDecision();
                            if (isContinueClicked)
                            {
                                isContinueClicked = false;
                                index++;
                                continue;
                            }
                        }

                        return StartupPipelineResult.Fail(result.ErrorId ?? "TASK_FAILED", $"Task {taskName} failed. {result.Message}");
                    }
                    else
                    {
                        Debug.Log($"[Startup] Task succeeded: {taskName}");          
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning($"[Startup] Task timeout or cancelled: {taskName}");
                    return StartupPipelineResult.Fail("TIMEOUT", $"Task {taskName} timed out.");
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    return StartupPipelineResult.Fail("EXCEPTION_" + ex.GetType().Name, ex.Message);
                }
            }

            index++;
        }

        return StartupPipelineResult.Ok();
    }

    bool isRetryClicked = false;
    bool isContinueClicked = false;
    private void OnNetworkRetryClicked() => isRetryClicked = true;
    private void OnContinueClicked() => isContinueClicked = true;

    private async Task WaitForNetworkDecision()
    {
        while (!isRetryClicked && !isContinueClicked)
        {
            await Task.Yield();
        }
    }

    private async void WaitForClick()
    {
        if (isCompleted && !isLoadingScene)
        {
            isLoadingScene = true;
            await SceneLoader.Instance.LoadScene("Title", true);
            inputActions.UI.Disable();
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
