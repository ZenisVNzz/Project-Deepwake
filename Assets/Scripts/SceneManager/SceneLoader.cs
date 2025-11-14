using Mirror;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{   
    public static SceneLoader Instance;
    [SerializeField] private Animator transitionAnimator;

    public List<SceneLoadKey> sceneLoadKeys;
    private InitScriptHelper initScriptHelper;

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

        initScriptHelper = GetComponent<InitScriptHelper>();
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

    public async Task LoadScene(string sceneName, bool initHelper)
    {
        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
        await LoadSceneProcess(sceneName, initHelper);        
    }

    private async Task LoadSceneProcess(string sceneName, bool initHelper)
    {
        AsyncOperation loadOperation = null;

        if (initHelper)
        {
            loadOperation = SceneManager.LoadSceneAsync("Loading");
            loadOperation.allowSceneActivation = false;

            foreach (var slk in sceneLoadKeys)
            {
                if (slk.sceneName == sceneName)
                {
                    initScriptHelper._preloadKeys = slk.preloadKeys;
                    break;
                }
            }

            initScriptHelper._loadSceneOnLoadDone = sceneName;

            while (loadOperation.progress < 0.9f)
                await Task.Yield();
        }
        else
        {
            loadOperation = SceneManager.LoadSceneAsync(sceneName);
            loadOperation.allowSceneActivation = false;
        }

        transitionAnimator.Play("Scene_Close");

        await WaitForAnimationEnd();

        loadOperation.allowSceneActivation = true;

        if (initHelper)
        {
            initScriptHelper.Load();
        }
    }

    private async Task WaitForAnimationEnd()
    {
        await Task.Yield();
        var animState = transitionAnimator.GetCurrentAnimatorStateInfo(0);
        while (animState.normalizedTime < 1f && animState.IsName("Scene_Close"))
        {
            await Task.Yield();
            animState = transitionAnimator.GetCurrentAnimatorStateInfo(0);
        }
    }

    [Serializable]
    public class SceneLoadKey
    {
        public string sceneName;
        public List<string> preloadKeys;
    }
}
