using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    public static CoroutineRunner Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void StopRunCoroutine(IEnumerator routine)
    {
        StopCoroutine(routine);
    }
}
