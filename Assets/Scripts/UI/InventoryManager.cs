using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; 
    public GameObject slotPrefab; 
    public Transform slotParent;
    public Player player;
    private List<ItemData> inventory; 

    private void Awake()
    {
        instance = this;
    }

    public void AddItem(Item newItem)
    {
        UpdateUI();
    }

    //public void UpdateUI()
    //{
    //    inventory = player.GetInventory();


    //    foreach (Transform child in slotParent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (var itemData in inventory)
    //    {
    //        ItemFactory.Instance.CreateItemLogicOnlyById(itemData.id, (item) =>
    //        {
    //            if (item == null)
    //            {
    //                Debug.LogError($"❌ 无法根据 ID {itemData.id} 创建 UI 用的 Item");
    //                return;
    //            }

    //            item.quantity = itemData.quantity;

    //            GameObject slot = Instantiate(slotPrefab, slotParent);
    //            slot.GetComponent<UIItemSlot>().SetItem(item);
    //        });
    //    }
    //}

    public void UpdateUI()
    {
        inventory = player.GetInventory();

        // 🧹 清空旧的 UI
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        if (inventory == null || inventory.Count == 0)
            return;

        // 🌱 准备缓存结构
        int itemCount = inventory.Count;
        Item[] itemCache = new Item[itemCount];
        int pendingCount = itemCount;

        for (int i = 0; i < itemCount; i++)
        {
            int index = i; // 防止闭包 capture 错乱
            var itemData = inventory[index];

            ItemFactory.Instance.CreateItemLogicOnlyById(itemData.id, (item) =>
            {
                if (item == null)
                {
                    Debug.LogError($"❌ 无法根据 ID {itemData.id} 创建 UI 用的 Item");
                    pendingCount--;
                    return;
                }

                item.quantity = itemData.quantity;
                itemCache[index] = item;
                pendingCount--;

                // ✅ 所有物品加载完毕，统一刷新 UI
                if (pendingCount == 0)
                {
                    for (int j = 0; j < itemCache.Length; j++)
                    {
                        var slot = Instantiate(slotPrefab, slotParent);
                        slot.GetComponent<UIItemSlot>().SetItem(itemCache[j]);
                    }
                }
            });
        }
    }


}
