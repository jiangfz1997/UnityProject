using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : IDamageHandler
{
    private Player player;
    private Rigidbody2D rb;
    private Transform transform;
    private Dictionary<DamageType, System.Action<Transform, float>> damageEffects;
    private SpriteRenderer spriteRenderer;
    private Material material;
    private DamageEffectHandler effectHandler;
    public DamageHandler(Player player, DamageEffectHandler effectHandler)
    {
        this.player = player;
        rb = player.GetComponent<Rigidbody2D>();
        this.effectHandler = effectHandler;
        transform = player.transform;

        damageEffects = new Dictionary<DamageType, System.Action<Transform, float>>
        {
            { DamageType.Physical, ApplyPhysicalDamage },
            { DamageType.Poison, (attacker, damage) => player.StartCoroutine(ApplyPoisonEffect(5f, 1f)) },
            { DamageType.Lava, ApplyLavaDamage }
        };
    }

    public void HandleDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        if (damageEffects.TryGetValue(damageType, out var effect))
        {
            effect.Invoke(attacker, knockbackForce);
        }
    }

    private void ApplyPhysicalDamage(Transform attacker, float knockbackForce)
    {
        effectHandler.TriggerEffect();
        ApplyKnockback(attacker, knockbackForce);
        player.TriggerInvincible(2f);

    }

    private void ApplyKnockback(Transform attacker, float knockbackForce)
    {
        Vector2 forceDirection = ((Vector2)transform.position - (Vector2)attacker.position).normalized;
        if (forceDirection == Vector2.zero)
        {
            forceDirection = new Vector2(0.1f, 0.1f).normalized;
        }

        Vector2 force = forceDirection * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    private void ApplyLavaDamage(Transform attacker, float knockbackForce)
    {
        player.TriggerInvincible(0.5f);
    }
    private IEnumerator ApplyPoisonEffect(float duration, float damagePerSecond)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            player.currentHP = Mathf.Max(player.currentHP - damagePerSecond, 0);
            if (player.currentHP <= 0) player.Die();
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        Debug.Log("Poison effect applied");
    }


}
