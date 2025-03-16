using UnityEngine;

public class ProductGridManager : MonoBehaviour
{
    public GameObject productItemPrefab; // Reference to the ProductItem prefab
    public Transform gridContainer;      // The Grid container (the parent object)
    public Backpack backpack;            // 引用背包管理器

    public ProductItem.ProductData[] products = new ProductItem.ProductData[6]; // 6 products

    void Start()
    {
        RefreshProducts();
    }

    void InitializeProducts()
    {
        // 分开初始化药水和项链
        products[0] = new ProductItem.ProductData { icon = Resources.Load<Sprite>("decorations/decorations_373"), description = "Healing Potion", price = 100, type = ProductItem.ProductType.Potion };
        products[1] = new ProductItem.ProductData { icon = Resources.Load<Sprite>("decorations/decorations_373"), description = "Mana Potion", price = 120, type = ProductItem.ProductType.Potion };
        products[2] = new ProductItem.ProductData { icon = Resources.Load<Sprite>("decorations/decorations_376"), description = "Silver Necklace", price = 300, type = ProductItem.ProductType.Necklace };
        products[3] = new ProductItem.ProductData { icon = Resources.Load<Sprite>("decorations/decorations_76"), description = "Golden Necklace", price = 500, type = ProductItem.ProductType.Necklace };
        products[4] = new ProductItem.ProductData { icon = Resources.Load<Sprite>("decorations/decorations_77"), description = "Elixir of Power", price = 250, type = ProductItem.ProductType.Potion };
        products[5] = new ProductItem.ProductData { icon = Resources.Load<Sprite>("decorations/decorations_78"), description = "Magic Ring", price = 450, type = ProductItem.ProductType.Necklace };
    }

    void CreateProductItems()
    {
        foreach (var product in products)
        {
            if (product != null)
            {
                GameObject productItemGO = Instantiate(productItemPrefab, gridContainer);
                ProductItem productItem = productItemGO.GetComponent<ProductItem>();

                // **传递 ProductGridManager 以便移除商品**
                productItem.Setup(product, this);
            }
        }
    }

    public void RemoveProduct(ProductItem.ProductData product)
    {
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i] != null && products[i].description == product.description)
            {
                products[i] = null; // 移除已购买的商品
                break;
            }
        }

        // 更新商店显示
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject); // 清空当前商店商品
        }
        CreateProductItems(); // 重新创建未购买的商品
    }

    public void RefreshProducts()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject); // 清除旧 UI
        }

        InitializeProducts(); // 重新生成商品
        CreateProductItems(); // 更新 UI
    }
}
