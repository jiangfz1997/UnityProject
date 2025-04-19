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

    [SerializeField] private int currentLevelIndex = 0;

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
            Debug.Log("All levels completed!");
            CleanDontDestroyOnLoad();
            SceneManager.LoadScene("ClearMenu");
        } else {
            loadEventSO.RaiseLoadRequestEvent(breakRoomScene, breakRoomSpawnPoint, true);
        }

        
    }

    private void CleanDontDestroyOnLoad()
    {
        Destroy(GameObject.Find("MusicManager"));
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("PlayerCamera"));
        Destroy(GameObject.Find("EventSystem"));
        Destroy(GameObject.Find("ScenesManager"));
        Destroy(GameObject.Find("DelayedActionManager"));
        Destroy(GameObject.Find("[DOTween]"));
        Destroy(GameObject.Find("[Debug Updater]"));

    }

    public void LoadNextLevel(int transType)
    {
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex >= levelScenes.Count)
        {
            AllScenesFinished();
        }

        currentLevelIndex = nextLevelIndex;

        Debug.Log($"Loading level: {currentLevelIndex}");
        loadEventSO.RaiseLoadRequestEvent(levelScenes[currentLevelIndex], levelSpawnPoint[currentLevelIndex], true);
    }

    private void AllScenesFinished()
    {
        Debug.Log("All levels completed!");
        // clear something
        SceneManager.LoadScene("ClearMenu");
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
    public void SetCurrentLevelIndex(int index)
    {
        if (index >= 0 && index < levelScenes.Count)
        {
            currentLevelIndex = index;
        }
        else
        {
            Debug.LogWarning($"Invalid level index: {index}");
        }
    }

    public int GetHighestCompletedLevel()
    {
        return currentLevelIndex;
    }
}