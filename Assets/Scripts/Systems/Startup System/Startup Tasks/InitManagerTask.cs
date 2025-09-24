using Mirror.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "InitManagerTask", menuName = "StartupSystem/InitManagerTask")]
public class InitManagerTask : StartupTask
{
    [SerializeField] private string _managersRoot;
    [SerializeField] private List<MonoScript> _script;
    public override bool HasTimeout { get { return true; } }

    public override async Task<bool> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        Transform managerRoot = GameObject.Find(_managersRoot).transform;

        if (managerRoot == null)
        {
            Debug.LogError("[InitManagerTask] ManagersRoot is not assign yet");
            return false;
        }

        if (_script == null)
        {
            Debug.LogError("[InitManagerTask] Script is not assign yet");
            return false;
        }

        foreach (var script in _script)
        {
            Debug.Log($"[InitManagerTask] Initialize manager: {script.name}");

            var type = script.GetClass();

            var component = managerRoot.GetComponent(type);
            if (component == null)
            {
                component = managerRoot.gameObject.AddComponent(type);
            }

            IManager manager = (IManager)component;
            await manager.InitAsync();

            serviceRegistry.Register(component);
            Debug.Log($"[InitManagerTask] Initialize {script.name} successfully.");
        }    

        return true;
    }

    private void OnValidate()
    {
        List<MonoScript> scriptsToRemove = new List<MonoScript>();
        foreach (var script in _script)
        {
            var type = script.GetClass();

            if (!typeof(IManager).IsAssignableFrom(type))
            {
                Debug.LogError($"[InitManagerTask] {script.name} is not correct type. The type of script must implement IManager");
                scriptsToRemove.Add(script);
            }
        }
        foreach (var config in scriptsToRemove)
        {
            _script.Remove(config);
        }
    }
}
