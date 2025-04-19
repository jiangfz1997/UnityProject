using UnityEngine;

public class TrapDamage : Attack
{
    public bool onlyPlayer;
    public float damageInterval = 1f;
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        damageType = DamageType.Lava;
        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage(transform, damage, 0, damageType);
        }
    }
}
