using UnityEngine;
using System.IO;
using System.Collections.Generic;
[System.Serializable]
public class SaveDataWrapper
{
    public List<SaveEntry> entries = new();
}

[System.Serializable]
public class SaveEntry
{
    public string key;
    public string json;
}
public static class SaveSystem
{
    public static void SaveObject<T>(T data, string fileName)
    {
        if (data == null)
        {
            Debug.LogWarning("No data to save.");
            return;
        }
        string json = JsonUtility.ToJson(data, true);
        Debug.Log($"Serialized data: {json}");
        string path = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log($"Save path: {path}");
        File.WriteAllText(path, json);
    }

    public static T LoadObject<T>(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found at: " + path);
            return default;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    public static T LoadSpecificObject<T>(string fileName, string key)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found at: " + path);
            return default;
        }

        string fullJson = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<SaveDataWrapper>(fullJson);
        if (wrapper == null || wrapper.entries == null)
        {
            Debug.LogError("Failed to parse SaveDataWrapper.");
            return default;
        }

        var entry = wrapper.entries.Find(e => e.key == key);
        if (entry == null)
        {
            Debug.LogWarning($"Entry with key '{key}' not found.");
            return default;
        }

        return JsonUtility.FromJson<T>(entry.json);
    }


    private const string DefaultSaveFile = "savegame.json";

    public static void SaveGame()
    {
        var saveables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        SaveDataWrapper wrapper = new SaveDataWrapper();

        foreach (var mono in saveables)
        {
            if (mono is ISaveable saveable)
            {
                if (saveable is BuffSystem bs && !bs.ShouldSave) continue;
                object state = saveable.CaptureState();
                string json = JsonUtility.ToJson(state);

                wrapper.entries.Add(new SaveEntry
                {
                    key = saveable.SaveKey(),
                    json = json
                });
            }
        }

        SaveObject(wrapper, DefaultSaveFile);
        Debug.Log("Game saved.");
    }

    public static void LoadGame()
    {
        SaveDataWrapper wrapper = LoadObject<SaveDataWrapper>(DefaultSaveFile);
        if (wrapper == null)
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        var saveables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var mono in saveables)
        {
            if (mono is ISaveable saveable)
            {
                if (saveable is BuffSystem bs && !bs.ShouldSave) continue;
                var entry = wrapper.entries.Find(e => e.key == saveable.SaveKey());
                if (entry != null)
                {
                    object state = JsonUtility.FromJson(entry.json, saveable.CaptureState().GetType());
                    saveable.RestoreState(state);
                }
            }
        }

        Debug.Log("Game loaded.");
    }
}
