using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    [SerializeField] private Animator transitionAnimator;

    private ResourceManager _resourceManager;

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

        if (_resourceManager == null)
        {
            _resourceManager = ResourceManager.Instance;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        transitionAnimator.Play("Scene_Open");
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }    

    public async Task LoadScene(string sceneName)
    {
        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
        await LoadSceneProcess(sceneName);        
    }

    private async Task LoadSceneProcess(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f)
        {
            await Task.Yield();
        }

        transitionAnimator.Play("Scene_Close");

        await Task.Yield();
        var animState = transitionAnimator.GetCurrentAnimatorStateInfo(0);

        while (animState.normalizedTime < 1f && animState.IsName("Scene_Close"))
        {
            await Task.Yield();
            animState = transitionAnimator.GetCurrentAnimatorStateInfo(0);
        }
        
        loadOperation.allowSceneActivation = true;
        Debug.Log($"[SceneLoader] scene {sceneName} loaded");
    }    
}
