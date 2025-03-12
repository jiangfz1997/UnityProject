using UnityEngine;

public class TrapDamage : Attack
{
    public bool onlyPlayer;
    public float damageInterval = 1f; // 持续掉血间隔
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        damageType = DamageType.Lava;
        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            //character.SetDamageHandler(new ConfigurableDamage(false, false)); // 不击退、不无敌
            character.TakeDamage(transform, damage, 0, damageType);
        }
    }
}
