using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private Enemy_behaviour enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy_behaviour>(); // �ҵ�������Ϊ�ű�
        if (enemy == null)
        {
            Debug.LogError("EnemyHitbox �Ҳ��� Enemy_behaviour �����");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            Debug.Log("Enemy �� PlayerAttack ���У�");
            var attack = collision.GetComponent<PlayerAttack>();
            if (attack != null)
            {
                enemy.TakeDamage(attack.transform, attack.damage, attack.knockbackForce, attack.damageType);
            }
        }
    }
}