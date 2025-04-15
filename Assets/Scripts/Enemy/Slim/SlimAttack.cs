using UnityEngine;

public class SlimAttack : Attack
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
