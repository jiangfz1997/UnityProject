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

    public void UpdateUI()
    {
        inventory = player.GetInventory();


        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

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
    
}
