using UnityEngine;

public abstract class Projectile : BaseProjectile
{
   
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploded) return;

        var character = collision.GetComponent<Character>();
        if (character != null && character.CompareTag("Enemy"))
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            isExploded = true;
            playExplodeSound();
            rb.linearVelocity = Vector2.zero;
            OnHit(character);

            //Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            isExploded = true;
            playExplodeSound();
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("explode");
            //Destroy(gameObject);
        }
        else if (collision.CompareTag("Boss1"))
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            isExploded = true;
            playExplodeSound();
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("explode");

            //OnHit(collision.GetComponent<Character>());
            BossFSM boss = collision.GetComponent<BossFSM>();
            if (boss != null)
            {
                float finalDamage = damage;
                DamageType attackDamageType = damageType;
                boss.TakeDamage(finalDamage, attackDamageType);
            }
            //Destroy(gameObject);
        }
        
    }
    protected void playExplodeSound()
    {
        if (audioSource != null && explodeSFX != null)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.PlayOneShot(explodeSFX);
        }
    }


    protected override void OnHit(Character target)
    {
        animator.SetTrigger("explode");
        target.TakeDamage(transform, damage, knockbackForce, damageType);
    }
}
