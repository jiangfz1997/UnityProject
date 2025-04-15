using System.Collections;
using UnityEngine;

public class PlayerAttack : Attack
{
    public GameObject elementAttackEffect;
    public float knockbackForce;
    private BuffSystem buffSystem;
    private Player player;
    private static bool canHit = true;
    //private static bool isHitPauseActive = false;

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
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                player.OnHitEnemy?.Invoke(enemy);
                CameraAttackVibration cav = GetComponent<CameraAttackVibration>();
                if (cav != null) { cav.TriggerImpulse(); }

                if (canHit)
                {
                    canHit = false;
                    StartCoroutine(HitPause(0.1f, 0.0f));
                    ResetHitCooldown();

                }

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
    private IEnumerator ResetHitCooldown()
    {
        // 可以控制冷却时间，比如 0.2 秒内不能重复触发
        yield return new WaitForSeconds(0.1f);
        canHit = true;
    }
    IEnumerator HitPause(float duration, float slowdownFactor = 0f)
    {
        // 保存原始时间速度
        float originalTimeScale = Time.timeScale;

        // 暂停或减慢
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // 等待实际时间（不是游戏内时间）
        yield return new WaitForSecondsRealtime(duration);

        // 恢复正常
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = 0.02f;

    }
}
    
