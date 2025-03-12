using UnityEngine;

public class TrapDamage : Attack
{
    public bool onlyPlayer;
    public float damageInterval = 1f; // ������Ѫ���
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        damageType = DamageType.Lava;
        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            //character.SetDamageHandler(new ConfigurableDamage(false, false)); // �����ˡ����޵�
            character.TakeDamage(transform, damage, 0, damageType);
        }
    }
}
