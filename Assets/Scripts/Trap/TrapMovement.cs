using UnityEngine;

public class TrapMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector3[] waypoints;  // 预设的移动路径点

    private int currentTargetIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // 移动到当前目标点
        Vector3 target = waypoints[currentTargetIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // 到达目标点后切换到下一个目标
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        }
    }
}

