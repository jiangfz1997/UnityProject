using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    public Transform buffBarParent; // Buff UI 容器
    public List<BuffPrefabData> buffPrefabs; // **存 BuffType 对应的 Prefab**

    private Dictionary<BuffType, GameObject> buffUIElements = new Dictionary<BuffType, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void AddBuff(BuffType type, float duration)
    {
        if (buffUIElements.ContainsKey(type))
        {
            // ? 如果 Buff 已存在，刷新时间
            buffUIElements[type].GetComponent<BuffSlot>().UpdateDuration(duration);
        }
        else
        {
            // ? 选择正确的 BuffPrefab
            GameObject prefab = GetBuffPrefab(type);
            if (prefab == null) return;

            // ? 创建 Buff UI
            GameObject buffSlot = Instantiate(prefab, buffBarParent);
            buffSlot.GetComponent<BuffSlot>().Initialize(type, duration);

            buffUIElements[type] = buffSlot;
        }
    }

    public void RemoveBuff(BuffType type)
    {
        if (buffUIElements.ContainsKey(type))
        {
            Destroy(buffUIElements[type]);
            buffUIElements.Remove(type);
        }
    }

    private GameObject GetBuffPrefab(BuffType type)
    {
        foreach (BuffPrefabData data in buffPrefabs)
        {
            if (data.buffType == type)
            {
                return data.prefab;
            }
        }
        return null;
    }
}
