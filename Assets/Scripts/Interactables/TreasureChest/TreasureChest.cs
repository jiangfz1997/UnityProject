using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class TreasureChest : MonoBehaviour, IInteractable
{
    public GameObject eKeyPrompt;
    public TreasureChestType chestType; // 这个宝箱的类型
    private LootTableLoader lootTableLoader;

    public TreasureChestAnimator animatorController;
    public Transform spawnPoint;
    private bool isOpened = false;

    void Start()
    {
        animatorController = GetComponent<TreasureChestAnimator>();
        // 使用 FindFirstObjectByType 代替已过时的 FindObjectOfType
        lootTableLoader = LootTableLoader.Instance; // 🚀 直接从单例获取

        lootTableLoader.LoadLootTable(); // 🚀 运行时加载 JSON 配置
        if (eKeyPrompt)
        {
            eKeyPrompt.SetActive(false);
        }
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            eKeyPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            eKeyPrompt.SetActive(false);
        }
    }

    void OpenChest()
    {
        isOpened = true;
        animatorController.PlayOpenAnimation();
        eKeyPrompt.SetActive(false);
        SpawnLoot();
    }
    private void SpawnLoot()
    {
        List<LootItem> lootItems = lootTableLoader.GetLootTable(chestType.ToString()); // 🚀 转换 `enum` 为 `string`
        if (lootItems == null || lootItems.Count == 0)
        {
            Debug.LogWarning("No loot table found!");
            return;
        }

        foreach (var loot in lootItems)
        {
            if (Random.value <= loot.dropChance)
            {
                StartCoroutine(LoadAndSpawnItem(loot.prefabName));
            }
        }
    }
    public void Interact()
    {
        if (!isOpened)
        {
            OpenChest();
        }
    }
    private IEnumerator LoadAndSpawnItem(string prefabName)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(prefabName);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Failed to load prefab: {prefabName}");
        }
    }
}

