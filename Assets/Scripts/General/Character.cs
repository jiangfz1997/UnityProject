using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

public class Character:MonoBehaviour
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

    

    private void Update()
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

    public void TakeDamage(Transform attacker)
    {   
        float damage = attacker.GetComponent<Attack>().damage;
        if (isInvincible) {
            return;
        }

        Debug.Log(gameObject.name+"Take Damage: " + damage);
        currentHP = Mathf.Max(currentHP - damage, 0); // 确保 currentHP 不会低于 0
        if (currentHP <= 0)
        {   
            OnDie?.Invoke(transform);
        }
        else
        {
            TriggerInvincible();
            OnTakeDamage?.Invoke(attacker.transform);
        }
    }
    private void Start()
    {
        currentHP = maxHP;
    }

    private void TriggerInvincible() 
    {
        if (!isInvincible)
        {
            Debug.Log(gameObject.name + " is invincible");
            isInvincible = true;
            invincibleCounter = invincibleTime;
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + "Die");

        //Destroy(gameObject);
    }

    

}
