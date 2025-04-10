using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemPriceText; 
    public Button sellButton; 

    public void SetItemData(Sprite icon, int price)
    {
        itemIcon.sprite = icon;

        int displayPrice = Mathf.FloorToInt(price * 0.3f);

        itemPriceText.text = "$" + displayPrice.ToString();
    }

}
