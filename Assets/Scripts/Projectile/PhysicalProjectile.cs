using UnityEngine;
using UnityEngine.TextCore.Text;

public class PhysicalProjectile : Projectile
{
    protected override void OnHit(Character target)
    {
        target.TakeDamage(transform, damage, knockbackForce, damageType);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
