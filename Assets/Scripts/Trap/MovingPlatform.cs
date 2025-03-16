using System.Collections;
using TMPro;
using UnityEngine;

public class MovingPlatform : Attack // �̳� Attack�������˺��߼�
{
    public Transform bottomPoint; // ƽ̨��͵�
    public Transform topPoint;    // ƽ̨��ߵ�
    public float speed = 2f;      // ƽ̨�ƶ��ٶ�
    public float damageInterval = 0.5f; // ������Ѫ��ʱ����
    private bool movingUp = true; // �Ƿ������ƶ�
    private Transform player;     // ��¼��ǰ���
    private float nextDamageTime; // �����´��˺�ʱ��
    private bool isStoppedByPlayer = false; // �Ƿ�������赲
    private Coroutine damageCoroutine; // �˺�Э������

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // ȡ������
        rb.freezeRotation = true;

        if (bottomPoint != null)
        {
            bottomPoint.position = transform.position; // ��ʼ�� bottomPoint ����Ϊƽ̨��ǰ����
        }
        if (topPoint == null)
        {
            Debug.LogError("topPoint δ���ã����� Inspector ��ָ�� TopPoint��");
        }

        if (bottomPoint == null)
        {
            Debug.LogError("bottomPoint δ���ã����� Inspector ��ָ�� BottomPoint��");
        }
    }

    void FixedUpdate()
    {
        if (isStoppedByPlayer) return; // ���������赲����ͣ�ƶ�
        MovePlatform();
    }

    void MovePlatform()
    {
        Vector2 target = movingUp ? topPoint.position : bottomPoint.position;
        target.x = transform.position.x; // ǿ��Ŀ��λ�õ� X ������ƽ̨һ�£���ֹб���ƶ�
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //Debug.Log("bottom position=" + bottomPoint.position.y);
        //Debug.Log("top position=" + topPoint.position.y);
        //Debug.Log("targetPosition=" + target);
        //Debug.Log("currentPosition=" + transform.position);
        // ����Ŀ�������ƶ�
        if (Mathf.Abs(transform.position.y - target.y) < 0.1f)
        {
            movingUp = !movingUp;
            //Debug.Log("ƽ̨����ת���·���" + (movingUp ? "����" : "����"));
        }
    }

    //protected override void OnTriggerStay2D(Collider2D collision)
    //{
    //    var character = collision.GetComponent<Character>();
    //    if (character != null)
    //    {
    //        Debug.Log("�����Ȼ�ڴ��������ڣ�");
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
                    character.TakeDamage(transform, damage, 0, damageType); // �������������˺�
                }
                movingUp = true; // ������Һ����������ƶ�
                Debug.Log("ƽ̨������ң���ƽ̨�·�������������˺������ı䷽������");
            }
        }
    }
    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("����뿪�� MovingTrap��ֹͣ�˺�");
    //        other.transform.SetParent(null);
    //        isStoppedByPlayer = false;

    //        // ֹͣ�����˺�
    //        if (damageCoroutine != null)
    //        {
    //            Debug.Log("ֹͣ DamagePlayer Э��");
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
    //        // ƽ̨����ʱ������赲��ֹͣ�ƶ�
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
    //            Debug.Log("������뿪���˳� DamagePlayer Э��");
    //            break;
    //        }

    //        if (Time.time >= nextDamageTime)
    //        {
    //            nextDamageTime = Time.time + damageInterval;
    //            character.TakeDamage(transform, damage, 0, damageType);
    //        }
    //        yield return new WaitForSeconds(damageInterval);
    //    }
    //    Debug.Log("DamagePlayer Э�̽���");
    //    damageCoroutine = null;
    //}
}
