using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class TreasureChest : MonoBehaviour, IInteractable
{
    //public GameObject eKeyPrompt;
    public TreasureChestType chestType; 
    private LootTableLoader lootTableLoader;

    public TreasureChestAnimator animatorController;
    public Transform spawnPoint;
    private bool isOpened = false;

    public AudioClip openSFX;
    protected AudioSource audioSource;

    void Start()
    {
        animatorController = GetComponent<TreasureChestAnimator>();
        lootTableLoader = LootTableLoader.Instance; 

        lootTableLoader.LoadLootTable();

        audioSource = GetComponent<AudioSource>();
        //if (eKeyPrompt)
        //{
        //    eKeyPrompt.SetActive(false);
        //}
    }

    void Update()
    {

    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    //if (other.CompareTag("Player") && !isOpened)
    //    //{
    //    //    eKeyPrompt.SetActive(true);
    //    //}
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        eKeyPrompt.SetActive(false);
    //    }
    //}

    void OpenChest()
    {
        isOpened = true;
        gameObject.tag = "Untagged";

        animatorController.PlayOpenAnimation();
        //eKeyPrompt.SetActive(false);

        SpawnLoot();
        if (audioSource != null && openSFX != null)
        {
            audioSource.PlayOneShot(openSFX);
        }
        else
        {
            Debug.LogWarning("AudioSource or Open SFX is not assigned.");
        }

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

