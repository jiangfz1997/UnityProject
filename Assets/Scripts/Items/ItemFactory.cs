using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class ItemDataEntry
{
    public int id;
    public string address;
    public string type;
    public int price;
}

[System.Serializable]
public class ItemDataTable
{
    public List<ItemDataEntry> items;
}

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance;

    private Dictionary<int, string> idToAddress = new Dictionary<int, string>();
    private string jsonFileName = "ItemTable.json";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadItemTable();
    }

    private void LoadItemTable()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        if (!File.Exists(path))
        {
            Debug.LogError("❌ 找不到 ItemTable.json 文件: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        ItemDataTable dataTable = JsonUtility.FromJson<ItemDataTable>(json);

        foreach (var entry in dataTable.items)
        {
            idToAddress[entry.id] = entry.address;
        }

        Debug.Log($"✅ ItemFactory 初始化完成，共加载 {idToAddress.Count} 项");
    }

    //public void CreateItemById(int id, System.Action<GameObject> callback)
    //{
    //    if (!idToAddress.TryGetValue(id, out var address))
    //    {
    //        Debug.LogError("❌ 无法根据ID找到对应的Item地址: " + id);
    //        callback?.Invoke(null);
    //        return;
    //    }

    //    Addressables.InstantiateAsync(address).Completed += (handle) =>
    //    {
    //        if (handle.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            callback?.Invoke(handle.Result);
    //        }
    //        else
    //        {
    //            Debug.LogError($"❌ 加载 {address} 失败！");
    //            callback?.Invoke(null);
    //        }
    //    };
    //}
    public void CreateItemLogicOnlyById(int id, Action<Item> onComplete)
    {
        if (!idToAddress.TryGetValue(id, out var address))
        {
            onComplete?.Invoke(null);
            return;
        }

        Addressables.LoadAssetAsync<GameObject>(address).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefab = handle.Result;
                GameObject obj = GameObject.Instantiate(prefab);
                obj.SetActive(false); 
                Item item = obj.GetComponent<Item>();
                onComplete?.Invoke(item);
            }
            else
            {
                onComplete?.Invoke(null);
            }
        };
    }
    public void CreateItemById(int id, Action<Item> onComplete)
    {
        if (!idToAddress.TryGetValue(id, out var address))
        {
            Debug.LogError($"❌ 未找到对应 ID {id} 的 Address");
            onComplete?.Invoke(null);
            return;
        }

        Addressables.InstantiateAsync(address).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject obj = handle.Result;
                Item item = obj.GetComponent<Item>();
                if (item == null)
                {
                    Debug.LogError($"❌ 成功加载了 GameObject，但是它上面没有挂载 Item 脚本：{obj.name}");
                }
                else
                {
                    Debug.Log($"✅ 成功加载并获取 Item: {item.itemName}, id: {item.id}");
                }

                onComplete?.Invoke(item);
            }
            else
            {
                Debug.LogError("❌ Addressables 加载失败");
                onComplete?.Invoke(null);
            }
        };
    }
}