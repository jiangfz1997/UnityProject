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
        else if (collision.CompareTag("Boss1"))
        {
            // Debug.Log("PlayerAttackBoss: " + collision.name);
            BossFSM boss = collision.GetComponent<BossFSM>();
            if (boss != null)
            {
                float finalDamage = damage * attackMultiplier;
                boss.TakeDamage(finalDamage, attackDamageType);
            }

        }
        else if (collision.CompareTag("Boss2"))
        {
            PhantomFSM boss = collision.GetComponent<PhantomFSM>();
            if (boss != null)
            {
                float finalDamage = damage * attackMultiplier;
                boss.TakeDamage(finalDamage, attackDamageType);
            }

        }
    }
    private IEnumerator ResetHitCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        canHit = true;
    }
    IEnumerator HitPause(float duration, float slowdownFactor = 0f)
    {
        float originalTimeScale = Time.timeScale;

        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = 0.02f;

    }
}
    
