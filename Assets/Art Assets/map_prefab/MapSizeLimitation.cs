using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSizeLimiter : MonoBehaviour
{
    public int width = 32;  // Tilemap 的宽度（瓦片数量）
    public int height = 32; // Tilemap 的高度（瓦片数量）

    private Tilemap tilemap;

    void Start()
    {
        //tilemap = GetComponent<Tilemap>();

        //// 清空超出范围的瓦片
        //for (int x = -100; x < 100; x++)
        //{
        //    for (int y = -100; y < 100; y++)
        //    {
        //        if (x < 0 || x >= width || y < 0 || y >= height)
        //        {
        //            tilemap.SetTile(new Vector3Int(x, y, 0), null); // 删除超出范围的瓦片
        //        }
        //    }
        //}
    }
}