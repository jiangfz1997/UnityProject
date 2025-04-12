using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class InventorySaveData
{
    public List<ItemData> items = new();
}
public class PlayerInventory : MonoBehaviour, ISaveable
{
    public List<ItemData> inventory = new();
    private bool isAddingItem = false;


    public string SaveKey() => "PlayerInventory";

    public object CaptureState()
    {
        return new InventorySaveData
        {
            items = inventory.ToList()
        };
    }

    public void RestoreState(object state)
    {
        var data = state as InventorySaveData;
        if (data == null) return;

        inventory = data.items;
        InventoryManager.instance.UpdateUI(); 
    }

    public List<ItemData> GetInventory() => inventory;

    public void UseItem(Player player, int index)
    {
        if (index < 0 || index >= inventory.Count) return;

        ItemData itemData = inventory[index];
        if (itemData == null) return;

        ItemFactory.Instance.CreateItemLogicOnlyById(itemData.id, (item) =>
        {
            if (item == null)
            {
                Debug.LogError("Cannot generate new instance");
                return;
            }

            item.quantity = itemData.quantity;
            item.OnPickup(player);

            if (item.quantity <= 0)
            {
                inventory.RemoveAt(index);
                ReorganizeInventory();
            }

            InventoryManager.instance.UpdateUI();
        });
    }

    private void ReorganizeInventory()
    {


        for (int i = 0; i < inventory.Count - 1; i++)
        {
            if (inventory[i] == null)
            {


                inventory[i] = inventory[i + 1];
                inventory[i + 1] = null;


            }
        }


    }

    public void StoreItem(Item item, Action<bool> onComplete)
    {
        bool alreadyOwned = inventory.Exists(i => i.id == item.id);
        if (alreadyOwned)
        {
            Debug.Log($"You already have {item.itemName}£¬cannot have duplicated one");
            return;
        }

        if (inventory.Count < 4 && !isAddingItem)
        {
            isAddingItem = true;

            ItemFactory.Instance.CreateItemLogicOnlyById(item.id, (createdItem) =>
            {
                isAddingItem = false;
                if (createdItem != null)
                {
                    Item newItem = createdItem.GetComponent<Item>();
                    if (newItem != null)
                    {
                        inventory.Add(new ItemData { id = newItem.id, quantity = 1 });
                        InventoryManager.instance.AddItem(newItem);
                        onComplete?.Invoke(true);
                        Debug.Log($"Add item: {newItem.itemName} to the bag");
                        return;
                    }
                    else
                    {
                        Debug.LogError("Can not find new item failed");
                        onComplete?.Invoke(false);
                        return;
                    }
                }

                else
                {
                    Debug.LogError("Create Inventory failed");
                    onComplete?.Invoke(false);
                    return;
                }
            });
        }
        else
        {
            Debug.Log("Bag is full, cannot add new item");
        }
    }

    public void RemoveItem(int itemId, Action<bool> onComplete)
    {
        Debug.Log($"Trying to remove item: {itemId}");

        ItemData itemToRemove = inventory.Find(i => i.id == itemId);
        if (itemToRemove == null)
        {
            onComplete?.Invoke(false);
            return;
        }

        bool removed = inventory.Remove(itemToRemove);
        if (removed)
        {
            Debug.Log($"Item deleted ID: {itemId}");

            InventoryManager.instance.UpdateUI();

            onComplete?.Invoke(true);
        }
        else
        {
            Debug.LogError("Failed to delete item");
            onComplete?.Invoke(false);
        }
    }
}
