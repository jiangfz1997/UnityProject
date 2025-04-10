using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public Image itemIcon; // 物品图标
    public TextMeshProUGUI quantityText; // ✅ 物品数量文本
    //public void SetItem(Item item)
    //{
    //    if (item != null)
    //    {
    //        itemIcon.sprite = item.icon;
    //        itemIcon.enabled = true;
    //    }
    //    else
    //    {
    //        itemIcon.enabled = false;
    //    }
    //}
    public void SetItem(Item item)
    {
        if (itemIcon == null)
        {
            itemIcon = GetComponentInChildren<Image>();
        }

        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (item != null && item.icon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;

            // 显示数量
            quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
            quantityText.enabled = true;
        }
        else
        {
            itemIcon.enabled = false;
            quantityText.enabled = false;
        }
    }
}
