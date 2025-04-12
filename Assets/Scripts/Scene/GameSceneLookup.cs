using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSceneLookup : MonoBehaviour
{
    public static GameSceneLookup Instance;

    private Dictionary<string, GameSceneSO> sceneMap = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            LoadSceneDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadSceneDatabase()
    {
        Addressables.LoadAssetsAsync<GameSceneSO>("SceneData", null).Completed += handle =>
        {
            foreach (var sceneSO in handle.Result)
            {
                sceneMap[sceneSO.sceneName] = sceneSO;
                Debug.Log($"Loaded SceneSO: {sceneSO.sceneName}");
            }
        };
    }

    public GameSceneSO GetSceneSOByName(string name)
    {
        if (sceneMap.TryGetValue(name, out var so))
            return so;

        Debug.LogError($"SceneSO not found for: {name}");
        return null;
    }
}
