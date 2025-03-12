using UnityEngine;

public class BoarAttack : Attack
{
    public float knockbackForce = 10f;
    //public DamageType damageType = DamageType.Physical;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            //character.SetDamageHandler(new ConfigurableDamage(true, true, knockbackForce)); //  ‹…À∫ÛŒﬁµ– & ª˜ÕÀ
            character.TakeDamage(transform, damage, knockbackForce, damageType);
        }
    }
}