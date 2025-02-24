using UnityEngine;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour
{
    public Transform startPoint;    // 起点
    public Transform endPoint;      // 终点
    public GameObject[] prefabs;    // Prefab数组
    public float gridSize = 32f;    // Prefab的边长

    private List<Vector3> pathPoints = new List<Vector3>(); // 存储路径点

    void Start()
    {
        GeneratePath();
        SpawnPrefabsAlongPath();
    }

    void GeneratePath()
    {
        Vector3 currentPos = startPoint.position;
        Vector3 targetPos = endPoint.position;
        pathPoints.Add(currentPos);

        while (Vector3.Distance(currentPos, targetPos) > gridSize)
        {
            float dx = targetPos.x - currentPos.x;
            float dy = targetPos.y - currentPos.y;

            // 优先移动距离较大的方向
            if (Mathf.Abs(dx) > Mathf.Abs(dy))
            {
                // 水平方向移动
                currentPos.x += (dx > 0) ? gridSize : -gridSize;
                currentPos.y += 0;
            }
            else
            {
                // 垂直方向移动
                currentPos.y += (dy > 0) ? gridSize : -gridSize;
                currentPos.x += 0;
            }

            // 将新点加入路径
            pathPoints.Add(currentPos);
        }

        pathPoints.Add(targetPos); // 添加终点
    }

    void SpawnPrefabsAlongPath()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            Vector3 spawnPos = pathPoints[i];

            // 放置 Prefab
            Instantiate(prefabs[randomIndex], spawnPos, Quaternion.identity);
        }
    }

    // 在 Scene 视图中绘制辅助线
    void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count < 2) return;

        Gizmos.color = Color.red;
        for (int i = 1; i < pathPoints.Count; i++)
        {
            Gizmos.DrawLine(pathPoints[i - 1], pathPoints[i]);
        }
    }
}