using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;         
    public int quantity;

    public ItemData() { }

    public ItemData(int id, int quantity = 1)
    {
        this.id = id;
        this.quantity = quantity;
    }

    
}