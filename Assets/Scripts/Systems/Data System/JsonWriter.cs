using System.IO;
using UnityEngine;

public class JsonWriter
{
    private string FilePath;

    public JsonWriter(string filePath)
        { this.FilePath = filePath; }

    public void Write<T>(T data, string fileName)
    {
        string path = Path.Combine(FilePath, fileName);
        string Json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, Json);
    }    
}
