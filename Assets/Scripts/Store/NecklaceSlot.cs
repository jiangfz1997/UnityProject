using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NecklaceSlot : MonoBehaviour
{
    public Image itemIcon; 
    public TextMeshProUGUI itemPriceText; 
    public Button sellButton; 

    private ProductItem.ProductData storedNecklace; 

    public void StoreNecklace(ProductItem.ProductData necklace)
    {
        storedNecklace = necklace;
        UpdateUI();
    }

    public ProductItem.ProductData GetStoredNecklace()
    {
        return storedNecklace;
    }

    private void UpdateUI()
    {
        if (storedNecklace != null)
        {
            itemIcon.sprite = storedNecklace.icon;

            int displayPrice = Mathf.FloorToInt(storedNecklace.price * 0.3f);

            itemPriceText.text = "$" + displayPrice.ToString();
            sellButton.gameObject.SetActive(true); 
        }
        else
        {
            itemIcon.sprite = null;
            itemPriceText.text = "";
            sellButton.gameObject.SetActive(false); 
        }
    }


    public void ClearSlot()
    {
        storedNecklace = null;
        UpdateUI();
    }
}
