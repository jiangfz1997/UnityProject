using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public DamageType damageType;
    //public float attackRange;
    //public float attackRate;
    protected Character Owner;

    protected virtual void Start()
    {
        Owner = GetComponent<Character>();
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetDamageType(DamageType damageType)
    {
        this.damageType = damageType;
    }

    public int GetDamage() => damage;
    public DamageType GetDamageType() => damageType;

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

        //var character = collision.GetComponent<Character>();
        //if (character != null)
        //{
        //    character.TakeDamage(transform);
        //}
        
    }
   
}
