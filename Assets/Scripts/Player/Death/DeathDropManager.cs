
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DropGoldSaveData
{
    public Vector3 position;
    public int goldAmount;
    public string sceneName;
}

public class DeathDropManager : MonoBehaviour
{
    public static DeathDropManager Instance { get; private set; }

    [SerializeField] private GameObject deathDropPrefab;

    private static DropGoldSaveData memoryDropData = null; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    public void SaveLatestDrop(Vector3 position, int goldAmount)
    {
        memoryDropData = new DropGoldSaveData
        {
            position = position,
            goldAmount = goldAmount,
            sceneName = SceneManager.GetActiveScene().name
        };

        Debug.Log($"[DeathDropManager] (Memory) Saved drop: {goldAmount} gold at {position} in {memoryDropData.sceneName}");
    }

    public void TrySpawnDropInCurrentScene()
    {
        if (memoryDropData == null) return;

        Debug.Log($"[DeathDropManager] (Memory) Attempting to spawn drop in {memoryDropData.sceneName}");

        if (memoryDropData.sceneName != SceneManager.GetActiveScene().name) return;

        GameObject drop = Instantiate(deathDropPrefab, memoryDropData.position, Quaternion.identity);
        drop.GetComponent<DeathDrop>().goldAmount = memoryDropData.goldAmount;

        Debug.Log($"[DeathDropManager] (Memory) Restored drop: {memoryDropData.goldAmount} gold at {memoryDropData.position} in {memoryDropData.sceneName}");
    }

    public void ClearDrop()
    {
        memoryDropData = null;
        Debug.Log($"[DeathDropManager] (Memory) Cleared drop data");
    }
}
