using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public DamageType damageType;
    //public float attackRange;
    //public float attackRate;


    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

        //var character = collision.GetComponent<Character>();
        //if (character != null)
        //{
        //    character.TakeDamage(transform);
        //}
        
    }
   
}
