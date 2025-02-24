using System.IO;
using UnityEngine;

public static class JSONUtilityHelper
{
    /// <summary>
    /// Load json file
    /// </summary>
    public static T LoadJson<T>(string fileName)
    {
        string rawPath = Path.Combine(Application.streamingAssetsPath, fileName);

        string path = Path.GetFullPath(rawPath);

        if (!File.Exists(path))
        {
            Debug.LogError($"JSON file not found at {path}");
            return default;
        }
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(jsonData);
    }

    /// <summary>
    /// save json to file
    /// </summary>
    public static void SaveJson<T>(string fileName, T data)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        string jsonData = JsonUtility.ToJson(data, true); // √¿ªØ JSON
        File.WriteAllText(path, jsonData);
        Debug.Log($"Saved JSON to {path}");
    }
}
