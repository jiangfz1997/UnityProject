using UnityEngine;

public class TrapMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector3[] waypoints;  // Ԥ����ƶ�·����

    private int currentTargetIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // �ƶ�����ǰĿ���
        Vector3 target = waypoints[currentTargetIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // ����Ŀ�����л�����һ��Ŀ��
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        }
    }
}

