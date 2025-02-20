using UnityEngine;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour
{
    public Transform startPoint;    // ���
    public Transform endPoint;      // �յ�
    public GameObject[] prefabs;    // Prefab����
    public float gridSize = 32f;    // Prefab�ı߳�

    private List<Vector3> pathPoints = new List<Vector3>(); // �洢·����

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

            // �����ƶ�����ϴ�ķ���
            if (Mathf.Abs(dx) > Mathf.Abs(dy))
            {
                // ˮƽ�����ƶ�
                currentPos.x += (dx > 0) ? gridSize : -gridSize;
                currentPos.y += 0;
            }
            else
            {
                // ��ֱ�����ƶ�
                currentPos.y += (dy > 0) ? gridSize : -gridSize;
                currentPos.x += 0;
            }

            // ���µ����·��
            pathPoints.Add(currentPos);
        }

        pathPoints.Add(targetPos); // ����յ�
    }

    void SpawnPrefabsAlongPath()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            Vector3 spawnPos = pathPoints[i];

            // ���� Prefab
            Instantiate(prefabs[randomIndex], spawnPos, Quaternion.identity);
        }
    }

    // �� Scene ��ͼ�л��Ƹ�����
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