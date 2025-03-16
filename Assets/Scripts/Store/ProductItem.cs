using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductItem : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemDescription;
    public Button buyButton;
    public TextMeshProUGUI priceText;

    private ProductData product;
    private ProductGridManager gridManager;

    public void Setup(ProductData product, ProductGridManager gridManager)
    {
        this.product = product;
        this.gridManager = gridManager;

        itemIcon.sprite = product.icon;
        itemDescription.text = product.description;
        priceText.text = "$" + product.price.ToString();

        buyButton.onClick.RemoveAllListeners(); // 防止重复绑定
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    void OnBuyButtonClick()
    {
        if (product == null || gridManager == null)
        {
            Debug.LogError("Product or GridManager is null!");
            return;
        }

        Money userMoney = FindObjectOfType<Money>();
        if (userMoney == null)
        {
            Debug.LogError("Money component not found!");
            return;
        }

        Backpack backpack = FindObjectOfType<Backpack>();
        if (backpack == null)
        {
            Debug.LogError("Backpack component not found!");
            return;
        }

        if (userMoney.SpendGold(product.price)) // 如果金币足够
        {
            Debug.Log("购买成功: " + product.description);

            // **移除按钮监听，防止销毁后仍然触发**
            buyButton.onClick.RemoveAllListeners();

            // **从商店管理器中移除商品**
            gridManager.RemoveProduct(product);

            // **将物品添加到背包**
            backpack.AddToBackpack(product);

            // **删除这个商品的 UI 对象**
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("金币不足，无法购买 " + product.description);
        }
    }

    [System.Serializable]
    public class ProductData
    {
        public Sprite icon;
        public string description;
        public int price;
        public ProductType type;
    }

    public enum ProductType
    {
        Potion,
        Necklace
    }
}
