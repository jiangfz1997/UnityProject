using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private Enemy_behaviour enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy_behaviour>(); // 找到敌人行为脚本
        if (enemy == null)
        {
            Debug.LogError("EnemyHitbox 找不到 Enemy_behaviour 组件！");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            Debug.Log("Enemy 被 PlayerAttack 命中！");
            var attack = collision.GetComponent<PlayerAttack>();
            if (attack != null)
            {
                enemy.TakeDamage(attack.transform, attack.damage, attack.knockbackForce, attack.damageType);
            }
        }
    }
}