using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

[SerializeField] private SceneLoadEventSO loadEventSO;

    [SerializeField] private List<GameSceneSO> levelScenes = new List<GameSceneSO>();
    [SerializeField] private List<Vector3> levelSpawnPoint = new List<Vector3>();
    
    [SerializeField] private GameSceneSO breakRoomScene;
    [SerializeField] private Vector3 breakRoomSpawnPoint = Vector3.zero;

    private int currentLevelIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartLevel()
    {
        Debug.Log($"Restarting level: {currentLevelIndex}");
        loadEventSO.RaiseLoadRequestEvent(levelScenes[currentLevelIndex], levelSpawnPoint[currentLevelIndex], true);
    }

    public void CompleteLevel(int completedLevelIndex)
    {
        if (completedLevelIndex >= 0 && completedLevelIndex < levelScenes.Count)
        {
            currentLevelIndex = completedLevelIndex;
        }

        if (currentLevelIndex >= levelScenes.Count - 1)
        {
            // all clear
            Debug.Log("All levels completed!");
            // clear something
            SceneManager.LoadScene("ClearMenu");
        } else {
            loadEventSO.RaiseLoadRequestEvent(breakRoomScene, breakRoomSpawnPoint, true);
        }

        
    }

    // 从休息室进入下一关卡
    public void LoadNextLevel(int transType)
    {
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex >= levelScenes.Count)
        {
            nextLevelIndex = 0;
            Debug.Log("All levels completed but this should not happen");
        }

        currentLevelIndex = nextLevelIndex;

        Debug.Log($"Loading level: {currentLevelIndex}");
        loadEventSO.RaiseLoadRequestEvent(levelScenes[currentLevelIndex], levelSpawnPoint[currentLevelIndex], true);
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public int GetHighestCompletedLevel()
    {
        return currentLevelIndex;
    }
}