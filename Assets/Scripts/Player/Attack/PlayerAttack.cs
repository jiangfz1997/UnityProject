using UnityEngine;

public class PlayerAttack : Attack
{
    public GameObject elementAttackEffect;
    public float knockbackForce;
    private BuffSystem buffSystem;
    private Player player;
    protected override void Start()
    {
        player = transform.root.GetComponent<Player>();
    }
    protected void OnEnable()
    {
        player = transform.root.GetComponent<Player>();
    }

    //protected override void OnTriggerStay2D(Collider2D collision)
    //{
    //    var character = collision.GetComponent<Character>();
    //    if (elementAttackEffect != null)
    //    {

    //        elementAttackEffect.SetActive(damageType != DamageType.Physical);

    //    }
    //    if (player == null)
    //    {
    //        player = transform.root.GetComponent<Player>();
    //    }
    //    //character.SetDamageHandler(new ConfigurableDamage(true, true, knockbackForce)); 
    //    DamageType attackDamageType = player.buffSystem.GetCurrentDamageType();
    //    float attackMultiplier = player.buffSystem.GetAttackMultiplier();
    //    if (character != null)
    //    {

    //        float finalDamage = damage * attackMultiplier;

    //        character.TakeDamage(transform, finalDamage, knockbackForce, attackDamageType);

    //        //character.TakeDamage(transform, damage, knockbackForce, damageType);
    //    }
    //    else
    //    {
    //        // 对boss敌人的攻击
    //        Debug.Log("PlayerAttackBoss: " + collision.name);
    //        BossFSM boss = collision.GetComponent<BossFSM>();
    //        if (boss != null)
    //        {
    //            float finalDamage = damage * attackMultiplier;
    //            boss.TakeDamage(finalDamage, attackDamageType);
    //        }

    //    }
    //}
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();

        if (player == null)
        {
            player = transform.root.GetComponent<Player>();
        }

        DamageType attackDamageType = player.GetCurrentDamageType();
        float attackMultiplier = player.buffSystem.GetAttackMultiplier();

        if (character != null)
        {
            //character.SetDamageHandler(new ConfigurableDamage(true, true, knockbackForce)); 

            float finalDamage = damage * attackMultiplier;
            character.TakeDamage(transform, finalDamage, knockbackForce, attackDamageType);
            Enemy enemy = collision.GetComponent<Enemy>(); // 你要有 Enemy 脚本

            if (enemy != null)
            {
                player.OnHitEnemy?.Invoke(enemy); // 派发事件，给饰品或系统监听
            }
        }
        else
        {
            // 对boss敌人的攻击
            //Debug.Log("PlayerAttackBoss: " + collision.name);
            BossFSM boss = collision.GetComponent<BossFSM>();
            if (boss != null)
            {
                float finalDamage = damage * attackMultiplier;
                boss.TakeDamage(finalDamage, attackDamageType);
            }

        }
    }
}
    
