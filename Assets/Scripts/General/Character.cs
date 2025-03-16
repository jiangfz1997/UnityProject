using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{
    [Header("Basic Attribute")]
    public float maxHP;
    public float currentHP;


    [Header("Invincible")]
    public bool isInvincible;
    public float invincibleTime;
    private float invincibleCounter;

    //public UnityEvent<Transform> OnTakeDamage;
    public event Action<Transform, float, float, DamageType> OnTakeDamage;

    public UnityEvent<Transform> OnDie;
    public UnityEvent<Character> OnHealthChange;

    [Header("Damage Cooldown")]

    protected DamageHandler damageHandler;
    public BuffSystem buffSystem;
    protected virtual void Start()
    {
        currentHP = maxHP;
        OnHealthChange?.Invoke(this);
       
    }


    public virtual void SetCurrentSpeed(float speed) { }

    public virtual float GetCurrentSpeed() { return 0; }
    
    protected virtual void Update()
    {
        if (isInvincible)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter < 0)
            {
                isInvincible = false;
            }
        }
    }

    //public virtual void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    //{
    //    //float damage = attacker.GetComponent<Attack>().damage;
    //    if (isInvincible) return;

    //    currentHP = Mathf.Max(currentHP - damage, 0);
    //    if (currentHP <= 0)
    //    {
    //        OnDie?.Invoke(transform);
    //    }
    //    else
    //    {
    //        //TriggerInvincible();
    //        OnTakeDamage?.Invoke(attacker, damage, knockbackForce, damageType);
    //    }

    //    OnHealthChange?.Invoke(this);
    //}
    public abstract void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType);

    public abstract void ModifyHP(float amount);
    protected void InvokeTakeDamageEvent(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        OnTakeDamage?.Invoke(attacker, damage, knockbackForce, damageType);
    }

    internal void TriggerInvincible(float invincibleTime = -1)
    {
        if (!isInvincible)
        {
            Debug.Log(gameObject.name + " is invincible");
            isInvincible = true;
            if (invincibleTime == -1)
            {
                invincibleCounter = this.invincibleTime;
            }
            else
            {
                invincibleCounter = invincibleTime;
            }
        }
    }
    
    public virtual void Die()
    {
        Debug.Log(gameObject.name + " Die");
    }
}
