using System;
using Unity.VisualScripting;
using UnityEngine;

public class NecklacePack : MonoBehaviour
{
    public int maxCapacity = 1; 
    public GameObject necklaceSlotPrefab; 
    public Transform necklaceContainer; 
    public Money money; 
    public Alert alert; 

    private NecklaceSlot[] necklaceSlots; 

    void Start()
    {
        necklaceSlots = new NecklaceSlot[maxCapacity];
        UpdateNecklacePack();
    }

    public void AddToNecklacePack(ProductItem.ProductData necklace, Action<bool> onComplete)
    {
        for (int i = 0; i < maxCapacity; i++)
        {
            if (necklaceSlots[i] == null)
            {
                int productId = necklace.id;
                NecklaceFactory.GetNecklaceSOById(productId, (so) =>
                {
                    if (so != null)
                    {
                        Player.Instance.EquipNecklace(so);
                        necklaceSlots[i] = CreateNecklaceSlot(necklace, i);
                        onComplete?.Invoke(true);
                    }
                    else
                    {
                        Debug.LogWarning("无法找到对应的 NecklaceSO，ID: " + productId.ToString());
                        onComplete?.Invoke(false);
                    }
                });

                return; // 注意：for 循环中一旦找到了空位就退出，等回调处理结果
            }
        }
        alert.ShowAlert("You've already have one necklace!");
        // 如果所有格子都满了
        onComplete?.Invoke(false);
    }


    NecklaceSlot CreateNecklaceSlot(ProductItem.ProductData necklace, int index)
    {
        GameObject necklaceSlot = Instantiate(necklaceSlotPrefab, necklaceContainer);
        NecklaceSlot slotScript = necklaceSlot.GetComponent<NecklaceSlot>();

       
        slotScript.StoreNecklace(necklace);

     
        slotScript.sellButton.onClick.AddListener(() => OnSellButtonClick(index));

        return slotScript;
    }

  
    void OnSellButtonClick(int index)
    {
        if (necklaceSlots[index] != null)
        {
            NecklaceSlot slot = necklaceSlots[index];
            ProductItem.ProductData necklaceToSell = slot.GetStoredNecklace();

            if (necklaceToSell != null)
            {
                int sellPrice = Mathf.FloorToInt(necklaceToSell.price * 0.3f);

                money.AddGold(sellPrice);
                Player.Instance.UnequipNecklace();
                Debug.Log("卖出项链: " + necklaceToSell.description + " 获得: " + sellPrice + "金币");

                slot.ClearSlot(); 
                necklaceSlots[index] = null; 
            }
        }
    }

    void UpdateNecklacePack() 
    {
        if (Player.Instance.HasNecklaceEquipped()) 
        {
            Necklace equippedNecklace = Player.Instance.GetEquippedNecklace();
            var product = ProductItemFactory.Instance.GetProductById(equippedNecklace.Id);
            necklaceSlots[0] = CreateNecklaceSlot(product, 0);


        }

    }

}
