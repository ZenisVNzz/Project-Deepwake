using System.IO;
using UnityEngine;

public class JsonReader
{
    private string FilePath;

    public JsonReader(string path)
        { this.FilePath = path; }

    public string Read(string fileName)
    {
        string path = Path.Combine(FilePath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[JsonReader] File not found: {path}");
            return default;
        }

        return File.ReadAllText(path);
    }
}
