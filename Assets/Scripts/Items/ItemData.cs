using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;          // Ψһ ID�����ڴ� ItemFactory ������Ӧ����Ʒ
    public int quantity;    // ������Ĭ��Ϊ 1��

    public ItemData() { }

    public ItemData(int id, int quantity = 1)
    {
        this.id = id;
        this.quantity = quantity;
    }
}