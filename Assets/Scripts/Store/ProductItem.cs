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

        buyButton.onClick.RemoveAllListeners();
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
        NecklacePack necklacePack = FindObjectOfType<NecklacePack>();

        if (backpack == null && necklacePack == null)
        {
            Debug.LogError("Neither Backpack nor NecklacePack component found!");
            return;
        }
        if (!userMoney.EnoughGold(product.price)) 
        {
            Debug.Log("Not enough gold");
            return;
        }


        bool canStore = false;


        if (product.type == ProductItem.ProductType.Potion)
        {
            backpack.AddToBackpack(product, (success) =>
            {
                if (success)
                {
                    userMoney.SpendGold(product.price); // ✅ 只有添加成功才扣钱
                    buyButton.onClick.RemoveAllListeners();
                    gridManager.RemoveProduct(product);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("❌ 购买失败，未扣钱！");
                }
            });
        }
        //else if (product.type == ProductItem.ProductType.Necklace)
        //{
        //    necklacePack.AddToNecklacePack(product, (success)


        //        );
        //    canStore = necklacePack.AddToNecklacePack(product);
        //}
        else if (product.type == ProductItem.ProductType.Necklace)
        {
            necklacePack.AddToNecklacePack(product, (success) =>
            {
                if (!success)
                {
                    Debug.Log("购买失败: " + product.description + "，槽位已满或资源加载失败");
                    return;
                }

                // ✅ 扣钱和清理逻辑放在成功回调里！
                if (userMoney.SpendGold(product.price))
                {
                    Debug.Log("购买成功: " + product.description);
                    buyButton.onClick.RemoveAllListeners();
                    gridManager.RemoveProduct(product);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("金币不足，无法购买 " + product.description);
                }
            });

            return; // 异步处理中，不执行下面逻辑
        }

        //if (!canStore)
        //{
        //    Debug.Log("购买失败: " + product.description + "，背包或项链槽已满");
        //    return;
        //}


        //if (userMoney.SpendGold(product.price))
        //{
        //    Debug.Log("购买成功: " + product.description);
        //    buyButton.onClick.RemoveAllListeners();
        //    gridManager.RemoveProduct(product);
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    Debug.Log("金币不足，无法购买 " + product.description);
        //}
    }

    


    [System.Serializable]
    public class ProductData
    {
        public int id;
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