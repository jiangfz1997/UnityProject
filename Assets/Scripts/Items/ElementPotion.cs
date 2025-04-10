using UnityEngine;
using System.Collections.Generic;
public class ElementPotion : Item
{
    public ElementType elementType;
    public float buffDuration;
    public float buffValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnPickup(Character character)
    {

        if (character == null)
        {
            return;
        }
        if (character is Player player)
        {
            base.OnPickup(character);
            player.AddElementPoint(elementType, 1);
            quantity--;
        }
    }
    public override Item Clone()
    {
        GameObject cloneObj = new GameObject("ClonedElementPotion_" + itemName);
        ElementPotion clone = cloneObj.AddComponent<ElementPotion>();

        // 拷贝基类字段
        clone.itemName = this.itemName;
        clone.icon = this.icon;
        clone.quantity = this.quantity;

        clone.elementType = this.elementType;

        cloneObj.hideFlags = HideFlags.HideAndDontSave;

        return clone;
    }

}
