using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProductItemFactory : MonoBehaviour
{
    public static ProductItemFactory Instance { get; private set; }
    public GameObject productItemPrefab;

    private Dictionary<int, ProductItem.ProductData> productLookup = new();

    [System.Serializable]
    public class ProductData
    {
        public int id;
        public string iconPath;
        public string description;
        public int price;
        public string type;
    }

    [System.Serializable]
    public class ProductDataTable
    {
        public List<ProductData> products;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProductTable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadProductTable()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ProductItemTable.json");
        if (!File.Exists(path))
        {
            Debug.LogError("❌ 找不到商品配置文件: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        ProductDataTable table = JsonUtility.FromJson<ProductDataTable>(json);

        foreach (var entry in table.products)
        {
            ProductItem.ProductData data = new ProductItem.ProductData
            {
                id = entry.id,
                icon = Resources.Load<Sprite>(entry.iconPath),
                description = entry.description,
                price = entry.price,
                type = Enum.TryParse(entry.type, out ProductItem.ProductType parsedType) ? parsedType : ProductItem.ProductType.Potion
            };

            productLookup[entry.id] = data;
        }

        Debug.Log($"✅ ProductItemFactory 加载完成，共 {productLookup.Count} 个商品");
    }

    public ProductItem.ProductData GetProductById(int id)
    {
        if (productLookup.TryGetValue(id, out var data))
        {
            return data;
        }

       
        Debug.LogWarning($"⚠️ 找不到商品 id: {id}");
        return null;
    }

    public List<ProductItem.ProductData> GetAllProducts()
    {
        return new List<ProductItem.ProductData>(productLookup.Values);
    }
}
