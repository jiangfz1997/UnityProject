using System.Collections.Generic;
using UnityEngine;

public class ProductGridManager : MonoBehaviour
{
    public GameObject productItemPrefab;
    public Transform gridContainer;
    public Backpack backpack;
    private Player player;


    public ProductItem.ProductData[] products = new ProductItem.ProductData[6];

    void Start()
    {
        RefreshProducts();
        player = Object.FindFirstObjectByType<Player>();

    }

    void OnEnable()
    {
        if (player == null)
        {
            player = Object.FindFirstObjectByType<Player>();
        }

        if (player != null)
        {
            //backpack.UpdateBackpack(player.GetInventory());
        }
        else
        {
            Debug.LogError("❌ Backpack: 无法在 OnEnable 中找到 Player！");
        }
    }

    void InitializeProducts()
    {

        products[0] = new ProductItem.ProductData { id = 1, icon = Resources.Load<Sprite>("decoration6"), description = "Health Potion(Large)\n-Restores 50% HP.\n", price = 100, type = ProductItem.ProductType.Potion };
        products[1] = new ProductItem.ProductData { id = 2, icon = Resources.Load<Sprite>("decoration5"), description = "Attack Potion(Large)\n-Attack Power 100% ↑.\n", price = 300, type = ProductItem.ProductType.Potion };
        products[2] = new ProductItem.ProductData { id = 3, icon = Resources.Load<Sprite>("decoration3"), description = "Dancer's Necklace\n-Movement Speed 5%↑\n-Attack Power 5% ↑", price = 300, type = ProductItem.ProductType.Necklace };
        products[3] = new ProductItem.ProductData { id = 4, icon = Resources.Load<Sprite>("decoration1"), description = "Brave Necklace\n-Enemies' movement speed 10%↓\n-Attack Speed 5% ↑", price = 500, type = ProductItem.ProductType.Necklace };
        products[4] = new ProductItem.ProductData { id = 5, icon = Resources.Load<Sprite>("decoration4"), description = "Defense Potion(Large)\n-Defense 70% ↑.\n", price = 250, type = ProductItem.ProductType.Potion };
        products[5] = new ProductItem.ProductData { id = 6, icon = Resources.Load<Sprite>("decoration2"), description = "Legendary Necklace\n- *An additional jump\n-Attack Speed 5% ↑", price = 450, type = ProductItem.ProductType.Necklace };
    }

    void CreateProductItems()
    {
        foreach (var product in products)
        {
            if (product != null)
            {
                GameObject productItemGO = Instantiate(productItemPrefab, gridContainer);
                ProductItem productItem = productItemGO.GetComponent<ProductItem>();


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
                products[i] = null;
                break;
            }
        }


        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        CreateProductItems();
    }

    public void RefreshProducts()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        InitializeProducts();
        CreateProductItems();
    }
}