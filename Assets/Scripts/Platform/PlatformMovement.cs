using UnityEngine;
using System.Collections.Generic;

public class PlatformMovement : MonoBehaviour
{
    public PlatformData settings;  // ��ѡ��Ĭ�ϵ��ƶ��ٶ�
    public float speed = 2.0f;  // �����ֶ����������δ������ʹ��Ĭ��ֵ
    public Vector3[] waypoints;  // �洢·���㣨�� Unity �������ֶ����ã�

    private int currentTargetIndex = 0;

    void Start()
    {
        // ��� speed δ�ֶ����ã���ʹ��Ĭ��ֵ
        if (settings != null)
        {
            speed = settings.speed;
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        // �ƶ�����ǰĿ���
        Vector3 target = waypoints[currentTargetIndex];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // ����Ŀ�����л�Ŀ��
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % waypoints.Length;
        }
    }
}
