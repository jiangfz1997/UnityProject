using UnityEngine;
using UnityEngine.UI;

public class Necklace : MonoBehaviour
{
    public Transform necklaceContainer; // 项链容器
    public GameObject productItemPrefab; // 购买的商品Prefab

    private bool necklacePurchased = false; // 检查项链是否已购买

    // 向项链区添加项链
    public void AddNecklace(ProductItem.ProductData product)
    {
        if (!necklacePurchased)
        {
            GameObject necklaceItemGO = Instantiate(productItemPrefab, necklaceContainer);
            ProductItem necklaceItem = necklaceItemGO.GetComponent<ProductItem>();
            //necklaceItem.Setup(product);
            //necklaceItem.SetupSellButton(HandleSellNecklace); // 传递卖项链的函数
            necklacePurchased = true;
        }
        else
        {
            Debug.Log("项链区已满，无法添加更多项链");
        }
    }

    // 卖掉项链
    void HandleSellNecklace(ProductItem.ProductData product)
    {
        Money userMoney = FindObjectOfType<Money>();
        userMoney.AddGold(product.price); // 增加金币
        necklacePurchased = false;
        //Destroy(product.gameObject); // 从项链区删除项链
    }
}
