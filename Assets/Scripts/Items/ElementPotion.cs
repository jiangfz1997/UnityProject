using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine.Audio;
public class ElementPotion : Item
{
    public ElementType elementType;
    public float buffDuration;
    public float buffValue;
    public override void OnUse(Character character)
    {

        if (character == null)
        {
            return;
        }
        if (character is Player player)
        {
            base.OnUse(character);
            Player.Instance.PlayDrinkSound();
            ElementSelectController.Instance.OpenElementSelect();
            //player.AddElementPoint(elementType, 1);
            //quantity--;
            if (!storable)
            {
                Destroy(gameObject);
            }
            //InventoryManager.instance.UpdateUI();

        }

    }
    public override Item Clone()
    {
        GameObject cloneObj = new GameObject("ClonedElementPotion_" + itemName);
        ElementPotion clone = cloneObj.AddComponent<ElementPotion>();

        clone.itemName = this.itemName;
        clone.icon = this.icon;
        clone.quantity = this.quantity;

        clone.elementType = this.elementType;

        cloneObj.hideFlags = HideFlags.HideAndDontSave;

        return clone;
    }

}
