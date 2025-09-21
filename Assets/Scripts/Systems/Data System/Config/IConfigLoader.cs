using UnityEngine;

public interface IConfigLoader
{
    T Load<T>() where T : ScriptableObject, IConfig;
}
