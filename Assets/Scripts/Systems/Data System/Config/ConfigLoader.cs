using UnityEngine;

public class ConfigLoader : IConfigLoader
{
    private readonly JsonReader reader;

    public ConfigLoader()
    {
        reader = new JsonReader(Application.persistentDataPath);
    }

    public void Load(ScriptableObject config)
    {
        if (!(config is IConfig))
        {
            Debug.LogWarning($"[ConfigLoader] {config.name} type is incorrect, please check again.");
            return;
        }    

        string fileName = config.name + ".json";

        string json = reader.Read(fileName);
        JsonUtility.FromJsonOverwrite(json, config);

    }
}
