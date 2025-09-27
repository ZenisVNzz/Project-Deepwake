using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EventManager : MonoBehaviour, IManager
{
    public static EventManager Instance;
    public Dictionary<string, Action> eventListeners = new Dictionary<string, Action>();

    public async Task<bool> InitAsync()
    {
        await Task.CompletedTask;
        return true;
    }

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
    }

    public void Register(string eventName, Action listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = null;
        }
        eventListeners[eventName] += listener;
    }

    public void Unregister(string eventName, Action listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] -= listener;
        }
    }

    public void Trigger(string eventName)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName]?.Invoke();
        }
    }
}
