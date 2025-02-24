using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Basic Attribute")]
    public float maxHP;
    public float currentHP;

    [Header("Invincible")]
    public bool isInvincible;
    public float invincibleTime;
    private float invincibleCounter;

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Transform> OnDie;
    public UnityEvent<Character> OnHealthChange;

    protected virtual void Start()
    {
        currentHP = maxHP;
        OnHealthChange?.Invoke(this);
    }

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

    public virtual void TakeDamage(Transform attacker)
    {
        float damage = attacker.GetComponent<Attack>().damage;
        if (isInvincible) return;

        currentHP = Mathf.Max(currentHP - damage, 0);
        if (currentHP <= 0)
        {
            OnDie?.Invoke(transform);
        }
        else
        {
            TriggerInvincible();
            OnTakeDamage?.Invoke(attacker.transform);
        }

        OnHealthChange?.Invoke(this);
    }

    protected void TriggerInvincible()
    {
        if (!isInvincible)
        {
            Debug.Log(gameObject.name + " is invincible");
            isInvincible = true;
            invincibleCounter = invincibleTime;
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " Die");
    }
}
