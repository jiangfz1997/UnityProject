using UnityEngine;

public class PurpleEliteAttack : Attack
{
    public float knockbackForce = 10f;
    //public DamageType damageType = DamageType.Physical;

    protected override void OnTriggerStay2D(Collider2D collision)
    {

        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage(transform, damage, knockbackForce, damageType);
        }
    }


}
