using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSizeLimiter : MonoBehaviour
{
    public int width = 32;  // Tilemap �Ŀ�ȣ���Ƭ������
    public int height = 32; // Tilemap �ĸ߶ȣ���Ƭ������

    private Tilemap tilemap;

    void Start()
    {
        //tilemap = GetComponent<Tilemap>();

        //// ��ճ�����Χ����Ƭ
        //for (int x = -100; x < 100; x++)
        //{
        //    for (int y = -100; y < 100; y++)
        //    {
        //        if (x < 0 || x >= width || y < 0 || y >= height)
        //        {
        //            tilemap.SetTile(new Vector3Int(x, y, 0), null); // ɾ��������Χ����Ƭ
        //        }
        //    }
        //}
    }
}