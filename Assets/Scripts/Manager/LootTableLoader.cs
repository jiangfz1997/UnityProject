using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public string prefabName;
    public float dropChance;
}

[System.Serializable]
public class LootTableEntry
{
    public string chestType;
    public List<LootItem> lootItems;
}

[System.Serializable]
public class LootTableData
{
    public List<LootTableEntry> lootTables;
}

public class LootTableLoader : MonoBehaviour
{
    public static LootTableLoader Instance;

    private string jsonFileName = "LootTable.json";
    private Dictionary<string, List<LootItem>> lootTables = new Dictionary<string, List<LootItem>>();
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
    private void Start()
    {
        LoadLootTable();
    }

    public void LoadLootTable()
    {
        LootTableData lootTableData = JSONUtilityHelper.LoadJson<LootTableData>(jsonFileName);
        if (lootTableData == null)
        {
            Debug.LogError("Failed to load loot table.");
            return;
        }

        lootTables.Clear();
        foreach (var entry in lootTableData.lootTables)
        {
            lootTables[entry.chestType] = entry.lootItems;
        }

        Debug.Log($"Loaded loot table with {lootTables.Count} chest types.");
    }

    public List<LootItem> GetLootTable(string chestType)
    {
        return lootTables.ContainsKey(chestType) ? lootTables[chestType] : null;
    }

    public void SaveLootTable()
    {
        throw new System.NotImplementedException();
    }
}
