using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Backpack : MonoBehaviour
{
    public int maxCapacity = 4; // 背包容量上限
    public GameObject itemSlotPrefab; // 背包物品的UI预制体
    public Transform backpackContainer; // 背包物品容器
    public Money money; // 引用Money管理器

    private ProductItem.ProductData[] backpackItems; // 存储背包物品的数据

    void Start()
    {
        backpackItems = new ProductItem.ProductData[maxCapacity]; // 初始化背包物品数组
    }

    // 向背包添加物品
    public bool AddToBackpack(ProductItem.ProductData product)
    {
        // 查找空位
        for (int i = 0; i < maxCapacity; i++)
        {
            if (backpackItems[i] == null)
            {
                backpackItems[i] = product;
                CreateItemSlot(product, i); // 创建UI展示背包物品
                return true; // 成功添加
            }
        }

        Debug.Log("背包已满，无法添加物品");
        return false; // 背包已满
    }

    // 创建背包物品UI
    void CreateItemSlot(ProductItem.ProductData product, int index)
    {
        GameObject itemSlot = Instantiate(itemSlotPrefab, backpackContainer);
        ItemSlot itemSlotScript = itemSlot.GetComponent<ItemSlot>();

        // 设置物品的图标和价格
        itemSlotScript.SetItemData(product.icon, product.price);

        // 设置卖出按钮的事件
        itemSlotScript.sellButton.onClick.AddListener(() => OnSellButtonClick(index));
    }

    // 卖出物品
    void OnSellButtonClick(int index)
    {
        if (backpackItems[index] != null)
        {
            ProductItem.ProductData productToSell = backpackItems[index];
            backpackItems[index] = null; // 清空背包位置

            // 卖出后增加金币
            money.AddGold(productToSell.price);

            // 更新UI：删除该物品的UI
            Destroy(backpackContainer.GetChild(index).gameObject);

            Debug.Log("卖出物品: " + productToSell.description + " 获得: " + productToSell.price + "金币");
        }
    }
}

