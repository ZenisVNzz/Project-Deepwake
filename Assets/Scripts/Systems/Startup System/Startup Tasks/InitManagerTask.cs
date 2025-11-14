using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "InitManagerTask", menuName = "StartupSystem/InitManagerTask")]
public class InitManagerTask : StartupTask
{
    [SerializeField] private string _managersRoot;

#if UNITY_EDITOR
    [SerializeField] private List<MonoScript> _script;
#endif

    [SerializeField, HideInInspector] private List<string> _managerTypeNames = new();

    public override bool HasTimeout => true;

    public override async Task<StartupTaskResult> RunTaskAsync(IServiceRegistry serviceRegistry, CancellationToken ct)
    {
        Transform managerRoot = GameObject.Find(_managersRoot)?.transform;
        if (managerRoot == null)
        {
            Debug.LogError("[InitManagerTask] ManagersRoot is not assigned.");
            return StartupTaskResult.Fail("MANAGERS_ROOT_NOT_FOUND", "ManagersRoot is not assigned.");
        }

        foreach (string typeName in _managerTypeNames)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                Debug.LogError($"[InitManagerTask] Cannot find type: {typeName}");
                continue;
            }

            var component = managerRoot.GetComponent(type);
            if (component == null)
                component = managerRoot.gameObject.AddComponent(type);

            IManager manager = (IManager)component;
            await manager.InitAsync();
            serviceRegistry.Register(component);
        }

        return StartupTaskResult.Ok();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        List<MonoScript> scriptsToRemove = new();
        _managerTypeNames.Clear();

        foreach (var script in _script)
        {
            var type = script.GetClass();
            if (!typeof(IManager).IsAssignableFrom(type))
            {
                Debug.LogError($"[InitManagerTask] {script.name} is not correct type. Must implement IManager.");
                scriptsToRemove.Add(script);
            }
            else
            {
                _managerTypeNames.Add(type.AssemblyQualifiedName);
            }
        }

        foreach (var config in scriptsToRemove)
            _script.Remove(config);
    }
#endif
}
