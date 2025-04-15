using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
[System.Serializable]
public class DamageHandler : IDamageHandler
{
    private Character target;
    private Rigidbody2D rb;
    private Transform transform;
    private Dictionary<DamageType, System.Action<Transform, float, float>> damageEffects;
    private SpriteRenderer spriteRenderer;
    private Material material;
    private DamageEffectHandler effectHandler;

    private Dictionary<DamageType, float> damageMultipliers = new Dictionary<DamageType, float>
    {
        { DamageType.Physical, 1.0f },
        { DamageType.Fire, 1.5f },  
        { DamageType.Ice, 1.2f },  
        { DamageType.Poison, 1.1f }
    };

    public DamageHandler(Character target, DamageEffectHandler effectHandler)
    {       
        this.target = target;
        rb = target.GetComponent<Rigidbody2D>();
        this.effectHandler = effectHandler;
        transform = target.transform;

        Debug.Log("Damage Handler Initialized");
        Debug.Log("Target: " + target);

        damageEffects = new Dictionary<DamageType, System.Action<Transform, float,float>>
        {
            { DamageType.Physical, ApplyPhysicalDamage },
            { DamageType.Lava, ApplyLavaDamage },
            { DamageType.Fire, ApplyFireDamage},
            { DamageType.Ice, ApplyIceDamage}
        };
    }

    private void ApplyFireDamage(Transform transform, float damage, float knockbackForce)
    {
        var effect = StatusEffectFactory.CreateEffect(StatusEffect.Burning, target);
        if (effect != null)
        {
            target.statusEffectSystem.AddEffect(effect);
        }
    }


    private void ApplyIceDamage(Transform transform, float damage, float knockbackForce)
    {
        var effect = StatusEffectFactory.CreateEffect(StatusEffect.Frozen, target);
        if (effect != null)
        {
            target.statusEffectSystem.AddEffect(effect);
        }
    }
    public void HandleDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        if (damageEffects.TryGetValue(damageType, out var effect))
        {
            float finalDamage = CalculateFinalDamage(target, damage, damageType);
            target.ModifyHP(-finalDamage);
            //HitStop.Instance.StopTime(0.1f, 0.05f);
            BloodManager.Instance.SpawnBlood(transform.position);
            effect.Invoke(attacker, finalDamage, knockbackForce);
        }
    }
    private IEnumerator ApplyBurnEffect(float duration, float damagePerSecond)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.ModifyHP(-damagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
    }
    private float CalculateFinalDamage(Character target, float damage, DamageType damageType)
    {
        float multiplier = damageMultipliers.ContainsKey(damageType) ? damageMultipliers[damageType] : 1.0f;
        float finalDamage = damage * multiplier;
        return finalDamage;
    }

    private void ApplyPhysicalDamage(Transform attacker, float finalDamage,  float knockbackForce)
    {
        this.effectHandler.TriggerEffect();
        ApplyKnockback(attacker, knockbackForce);
        target.TriggerInvincible(target.invincibleTime);

    }

    private void ApplyKnockback(Transform attackerTransform, float knockbackForce)
    {
        //Transform attackerTransform = attacker.transform;
        Vector2 forceDirection = ((Vector2)transform.position - (Vector2)attackerTransform.position).normalized;
        if (forceDirection == Vector2.zero)
        {
            forceDirection = new Vector2(0.1f, 0.1f).normalized;
        }

        Vector2 force = forceDirection * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    private void ApplyLavaDamage(Transform attacker, float finalDamage, float knockbackForce)
    {
        target.TriggerInvincible(0.5f);
    }


    //private IEnumerator ApplyPoisonEffect(float duration, float finalDamage, float damagePerSecond)
    //{
    //    float elapsed = 0f;
    //    while (elapsed < duration)
    //    {
    //        target.currentHP = Mathf.Max(target.currentHP - damagePerSecond, 0);
    //        if (target.currentHP <= 0) target.Die();
    //        yield return new WaitForSeconds(1f);
    //        elapsed += 1f;
    //    }
    //    Debug.Log("Poison effect applied");
    //}


}
