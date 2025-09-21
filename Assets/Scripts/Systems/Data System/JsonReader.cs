using System.IO;
using UnityEngine;

public class JsonReader
{
    private string FilePath;

    public JsonReader(string path)
        { this.FilePath = path; }

    public T Read<T>(string fileName)
    {
        string path = Path.Combine(FilePath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[JsonReader] File not found: {path}");
            return default;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}
