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
        //inventory.Add(newItem);
        UpdateUI();
    }

    public void UpdateUI()
    {
        inventory = player.GetInventory();

        // 清除旧的 UI
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        // 遍历每个 ItemData，异步生成真实 Item 后显示
        foreach (var itemData in inventory)
        {
            ItemFactory.Instance.CreateItemLogicOnlyById(itemData.id, (item) =>
            {
                if (item == null)
                {
                    Debug.LogError($"❌ 无法根据 ID {itemData.id} 创建 UI 用的 Item");
                    return;
                }

                item.quantity = itemData.quantity;

                GameObject slot = Instantiate(slotPrefab, slotParent);
                slot.GetComponent<UIItemSlot>().SetItem(item);
            });
        }
    }
    //public void UpdateUI()
    //{
    //    for (int i = 0; i < slotParent.childCount; i++)
    //    {
    //        UIItemSlot slot = slotParent.GetChild(i).GetComponent<UIItemSlot>();

    //        if (i < player.inventory.Length)
    //        {
    //            slot.SetItem(player.inventory[i]); // ✅ 直接更新固定槽位
    //        }
    //    }
    //}
}
