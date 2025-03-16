using System.Collections;
using TMPro;
using UnityEngine;

public class MovingPlatform : Attack // 继承 Attack，复用伤害逻辑
{
    public Transform bottomPoint; // 平台最低点
    public Transform topPoint;    // 平台最高点
    public float speed = 2f;      // 平台移动速度
    public float damageInterval = 0.5f; // 持续掉血的时间间隔
    private bool movingUp = true; // 是否向上移动
    private Transform player;     // 记录当前玩家
    private float nextDamageTime; // 计算下次伤害时间
    private bool isStoppedByPlayer = false; // 是否因玩家阻挡
    private Coroutine damageCoroutine; // 伤害协程引用

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 取消重力
        rb.freezeRotation = true;

        if (bottomPoint != null)
        {
            bottomPoint.position = transform.position; // 初始化 bottomPoint 坐标为平台当前坐标
        }
        if (topPoint == null)
        {
            Debug.LogError("topPoint 未设置！请在 Inspector 里指定 TopPoint。");
        }

        if (bottomPoint == null)
        {
            Debug.LogError("bottomPoint 未设置！请在 Inspector 里指定 BottomPoint。");
        }
    }

    void FixedUpdate()
    {
        if (isStoppedByPlayer) return; // 如果因玩家阻挡，暂停移动
        MovePlatform();
    }

    void MovePlatform()
    {
        Vector2 target = movingUp ? topPoint.position : bottomPoint.position;
        target.x = transform.position.x; // 强制目标位置的 X 坐标与平台一致，防止斜着移动
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //Debug.Log("bottom position=" + bottomPoint.position.y);
        //Debug.Log("top position=" + topPoint.position.y);
        //Debug.Log("targetPosition=" + target);
        //Debug.Log("currentPosition=" + transform.position);
        // 到达目标点后反向移动
        if (Mathf.Abs(transform.position.y - target.y) < 0.1f)
        {
            movingUp = !movingUp;
            //Debug.Log("平台方向反转！新方向：" + (movingUp ? "向上" : "向下"));
        }
    }

    //protected override void OnTriggerStay2D(Collider2D collision)
    //{
    //    var character = collision.GetComponent<Character>();
    //    if (character != null)
    //    {
    //        Debug.Log("玩家仍然在触发区域内！");
    //        damageType = DamageType.Physical;
    //        if (damageCoroutine == null)
    //        {
    //            damageCoroutine = StartCoroutine(DamagePlayer(character));
    //        }
    //    }
    //}
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !movingUp)
        {
            Vector2 playerPos = collision.transform.position;
            Vector2 platformPos = transform.position;
            if (playerPos.y < platformPos.y)
            {
                var character = collision.gameObject.GetComponent<Character>();
                if (character != null)
                {
                    character.TakeDamage(transform, damage, 0, damageType); // 立即对玩家造成伤害
                }
                movingUp = true; // 碰到玩家后立刻向上移动
                Debug.Log("平台遇到玩家（在平台下方），立即造成伤害，并改变方向向上");
            }
        }
    }
    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("玩家离开了 MovingTrap，停止伤害");
    //        other.transform.SetParent(null);
    //        isStoppedByPlayer = false;

    //        // 停止持续伤害
    //        if (damageCoroutine != null)
    //        {
    //            Debug.Log("停止 DamagePlayer 协程");
    //            StopCoroutine(damageCoroutine);
    //            damageCoroutine = null;
    //        }
    //    }
    //}

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && !movingUp && !isStoppedByPlayer)
    //    {
    //        Debug.Log("====OnCollisionEnter2D====");
    //        // 平台下移时被玩家阻挡，停止移动
    //        isStoppedByPlayer = true;
    //        player = collision.transform;

    //        var character = collision.gameObject.GetComponent<Character>();
    //        if (character != null && damageCoroutine == null)
    //        {
    //            Debug.Log("====OnCollisionEnter2D damage====");
    //            damageCoroutine = StartCoroutine(DamagePlayer(character));
    //        }
    //    }
    //}

    //IEnumerator DamagePlayer(Character character)
    //{
    //    while (character != null && isStoppedByPlayer)
    //    {
    //        if (character == null || !isStoppedByPlayer)
    //        {
    //            Debug.Log("玩家已离开，退出 DamagePlayer 协程");
    //            break;
    //        }

    //        if (Time.time >= nextDamageTime)
    //        {
    //            nextDamageTime = Time.time + damageInterval;
    //            character.TakeDamage(transform, damage, 0, damageType);
    //        }
    //        yield return new WaitForSeconds(damageInterval);
    //    }
    //    Debug.Log("DamagePlayer 协程结束");
    //    damageCoroutine = null;
    //}
}
