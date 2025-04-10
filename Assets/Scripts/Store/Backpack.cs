using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class Backpack : MonoBehaviour
{
    public int maxCapacity = 4; 
    public GameObject itemSlotPrefab; 
    public Transform backpackContainer; 
    public Money money; 
    public Alert alert;
    private Player player;

    private ProductItem.ProductData[] backpackItems; 

    void Start()
    {
        backpackItems = new ProductItem.ProductData[maxCapacity];
        player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            UpdateBackpack(player.GetInventory());
            money.UpdateMoneyText(player.GetGold());
        }
        else
        {
            Debug.LogError("❌ Backpack: 无法在 OnEnable 中找到 Player！");
        }
    }

    void OnEnable()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
        }

        if (player != null)
        {
            UpdateBackpack(player.GetInventory());
        }
        else
        {
            Debug.LogError("❌ Backpack: 无法在 OnEnable 中找到 Player！");
        }
    }

    public void AddToBackpack(ProductItem.ProductData product, Action<bool> onComplete)
    {
        if (product.type == ProductItem.ProductType.Potion)
        {
            for (int i = 0; i < maxCapacity; i++)
            {
                if (backpackItems[i] == null)
                {
                    ItemFactory.Instance.CreateItemLogicOnlyById(product.id, (obj) =>
                    {
                        if (obj != null)
                        {
                            //obj.transform.position = new Vector3(-50, -50, -50);

                            player.StoreItem(obj.GetComponent<Item>(), (success) =>
                            {
                                if (success)
                                {
                                    backpackItems[i] = product;
                                    UpdateBackpack(player.GetInventory());
                                    onComplete?.Invoke(true); // ✅ 添加成功
                                }
                                else
                                {
                                    alert.ShowAlert("❌ 添加到角色背包失败！");
                                    onComplete?.Invoke(false);
                                }
                            });
                        }
                        else
                        {
                            alert.ShowAlert("❌ 创建物品失败！");
                            onComplete?.Invoke(false);
                        }
                    });
                    return; // ⚠️ 提前返回，避免重复执行
                }
            }

            alert.ShowAlert("❌ 背包已满，无法添加！");
            onComplete?.Invoke(false);
            return;
        }

        onComplete?.Invoke(false);
    }
    void CreateItemSlot(ProductItem.ProductData product, int index)
    {
        GameObject itemSlot = Instantiate(itemSlotPrefab, backpackContainer);
        ItemSlot itemSlotScript = itemSlot.GetComponent<ItemSlot>();

        itemSlotScript.SetItemData(product.icon, product.price);

     
        itemSlotScript.sellButton.onClick.AddListener(() => OnSellButtonClick(index));
    }

    
    void OnSellButtonClick(int index)
    {
        if (backpackItems[index] != null)
        {
            ProductItem.ProductData productToSell = backpackItems[index];
            backpackItems[index] = null; 
            int sellPrice = Mathf.FloorToInt(productToSell.price * 0.3f);
            player.RemoveItem(productToSell.id, success =>
            {
                if (success)
                {
                    money.AddGold(sellPrice);

                    //Destroy(backpackContainer.GetChild(index).gameObject);
                    UpdateBackpack(player.GetInventory());
                    Debug.Log("卖出物品: " + productToSell.description + " 获得: " + productToSell.price + "金币");
                }
                else
                {
                    alert.ShowAlert("❌ 删除失败，无法卖出物品！");
                }
            });
            
        }
    }


    public void UpdateBackpack(List<ItemData> getInventory)
    {
        // 🧹 清空旧的背包格子
        if (backpackItems == null) return;
        foreach (Transform child in backpackContainer)
        {
            Destroy(child.gameObject);
        }

        // 🧹 重置数据结构（可选，看你是不是维护这个）
        for (int i = 0; i < backpackItems.Length; i++)
        {
            backpackItems[i] = null;
        }

        // 🧱 重新根据玩家的背包数据生成 UI 格子
        for (int i = 0; i < getInventory.Count; i++)
        {
            ItemData itemData = getInventory[i];
            if (itemData == null) continue;

            var product = ProductItemFactory.Instance.GetProductById(itemData.id);
            if (product != null)
            {
                CreateItemSlot(product, i);
                backpackItems[i] = product;
            }
            else
            {
                Debug.LogWarning($"⚠️ 无法为物品 {itemData.id} 匹配商品");
            }
        }
    }


}
