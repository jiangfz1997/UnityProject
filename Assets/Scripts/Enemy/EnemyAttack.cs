using UnityEngine;

public class BoarAttack : Attack
{
    public float knockbackForce = 10f;

    protected override void OnTriggerStay2D(Collider2D collision)
    {

        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage(transform, damage, knockbackForce, damageType);
        }
    }
}