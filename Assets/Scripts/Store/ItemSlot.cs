using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public Image itemIcon; // 物品图标
    public TextMeshProUGUI itemPriceText; // 物品价格文本
    public Button sellButton; // 卖出按钮

    // 设置背包物品的图标和价格
    public void SetItemData(Sprite icon, int price)
    {
        itemIcon.sprite = icon;
        itemPriceText.text = "$" + price.ToString();
    }
}
