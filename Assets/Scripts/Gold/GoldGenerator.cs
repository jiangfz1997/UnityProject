using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GoldGenerator : MonoBehaviour
{
    public static GoldGenerator Instance { get; private set; }
    public GameObject goldPrefab;
    public int goldPerCoin = 10; 
    public float explosionForce = 3f;
    public float explosionRadius = 1f; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject); 
        }
    }


    private void Start()
    {
        // ✅ 注册事件
        Addressables.LoadAssetAsync<GameObject>("GoldPrefab").Completed += OnGoldPrefabLoaded;
    }
    private void LoadGoldPrefab()
    { 
        Addressables.LoadAssetAsync<GameObject>("GoldPrefab").Completed += OnGoldPrefabLoaded;
    }
    private void OnGoldPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            goldPrefab = handle.Result;
        }
        else
        {
            Debug.LogError("GoldPrefab 加载失败");
        }
    }
    public void GenerateGolds(Vector3 position, int amount, bool isFromPlayer = false)
    {
        if (goldPrefab == null)
        {
            LoadGoldPrefab();
           
        }
        int coinCount = amount / goldPerCoin;
        int remainder = amount % goldPerCoin;

        for (int i = 0; i < coinCount; i++)
        {
            CreateGold(position, goldPerCoin, isFromPlayer);
        }

        // ✅ 处理最后一个金币的余数
        if (remainder > 0)
        {
            CreateGold(position, remainder, isFromPlayer);
        }
    }

    private void CreateGold(Vector3 position, int coinValue, bool isFromPlayer=false)
    {
        Vector3 spawnPosition = position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 0.8f), 0);
        GameObject gold = Instantiate(goldPrefab, spawnPosition, Quaternion.identity);
        gold.GetComponent<Gold>().Initialize(coinValue);
        float eForce = explosionForce;
        if (isFromPlayer)
        {
            gold.layer = LayerMask.NameToLayer("IgnorePlayer");
            eForce *= 4;
        }
       

        // ✅ 让金币 `爆炸飞出`
        Rigidbody2D rb = gold.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized; // ✅ 随机方向
            rb.AddForce(randomDirection * eForce, ForceMode2D.Impulse);
        }
    }
}
