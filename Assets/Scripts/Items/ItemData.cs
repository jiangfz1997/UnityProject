using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;          // 唯一 ID，用于从 ItemFactory 创建对应的物品
    public int quantity;    // 数量（默认为 1）

    public ItemData() { }

    public ItemData(int id, int quantity = 1)
    {
        this.id = id;
        this.quantity = quantity;
    }
}