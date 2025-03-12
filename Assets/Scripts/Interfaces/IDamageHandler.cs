using UnityEngine;

public interface IDamageHandler
{
    void HandleDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType);
}
