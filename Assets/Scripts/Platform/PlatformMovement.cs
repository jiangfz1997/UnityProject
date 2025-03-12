using UnityEngine;
using System.Collections.Generic;

public class PlatformMovement : MonoBehaviour
{
    public PlatformData settings;  // 可选：默认的移动速度
    public float speed = 2.0f;  // 可以手动调整，如果未设置则使用默认值
    public Vector3[] waypoints;  // 存储路径点（在 Unity 场景中手动设置）

    private int currentTargetIndex = 0;

    void Start()
    {
        // 如果 speed 未手动设置，则使用默认值
        if (settings != null)
        {
            speed = settings.speed;
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        // 移动到当前目标点
        Vector3 target = waypoints[currentTargetIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // 到达目标点后切换目标
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        }
    }
}
