using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigList", menuName = "DataSystem/ConfigList")]
public class ConfigList : ScriptableObject
{
    [SerializeField] private List<ScriptableObject> _configs = new List<ScriptableObject>();
    public List<ScriptableObject> Configs => _configs;

    private void OnValidate()
    {
        List<ScriptableObject> ObjToRemove = new List<ScriptableObject>();
        foreach (var config in _configs)
        {
            if (!(config is IConfig))
            {
                Debug.LogError($"[ConfigList] {config.name} is not correct type. The type of config must implement IConfig");
                ObjToRemove.Add(config);
            }    
        }    
        foreach (var config in ObjToRemove)
        {
            _configs.Remove(config);
        }    
    }
}
