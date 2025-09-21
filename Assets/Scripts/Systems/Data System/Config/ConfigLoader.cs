using UnityEngine;

public class ConfigLoader : IConfigLoader
{
    private readonly JsonReader reader;

    public ConfigLoader()
    {
        reader = new JsonReader(Application.persistentDataPath);
    }

    public T Load<T>() where T : ScriptableObject, IConfig
    {
        string fileName = typeof(T).Name + ".json";

        string json = reader.Read(fileName);

        T instance = ScriptableObject.CreateInstance<T>();

        JsonUtility.FromJsonOverwrite(json, instance);

        return instance;
    }
}
