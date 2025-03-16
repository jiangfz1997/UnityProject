using UnityEngine;

public class PlayerAttack : Attack
{
    public float knockbackForce;
    private BuffSystem buffSystem;
    private Player player;
    protected override void Start()
    {
        player = transform.root.GetComponent<Player>();
    }
    protected void OnEnable()
    {
        player = transform.root.GetComponent<Player>();
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();

        if (character != null)
        {
            if (player == null)
            {
                player = transform.root.GetComponent<Player>();
            }
            //character.SetDamageHandler(new ConfigurableDamage(true, true, knockbackForce)); 
            DamageType attackDamageType = player.buffSystem.GetCurrentDamageType();
            float attackMultiplier = player.buffSystem.GetAttackMultiplier();
            // ���������˺�
            float finalDamage = damage * attackMultiplier;
            character.TakeDamage(transform, finalDamage, knockbackForce, attackDamageType);

            //character.TakeDamage(transform, damage, knockbackForce, damageType);
        }
    }
}
