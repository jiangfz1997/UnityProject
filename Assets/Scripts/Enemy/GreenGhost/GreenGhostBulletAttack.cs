
using UnityEngine;

public class GreenGhostBullet : Attack
{
    public float knockbackForce = 2f;
    //public DamageType damageType = DamageType.Physical;

    protected void OnTriggerEnter2D(Collider2D collision)
    {

        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage(transform, damage, knockbackForce, damageType);
        }
        OnDestroy();

    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
