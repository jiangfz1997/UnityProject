using UnityEngine;

public class EnemyProjectile : BaseProjectile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploded) return;
        var character = collision.GetComponent<Character>();
        if (character == null) return;
        if (character.CompareTag("Player"))
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            isExploded = true;
            playExplodeSound();
            rb.linearVelocity = Vector2.zero;
            OnHit(character);
            DestroySelf(0f);

        }
        else if (collision.CompareTag("Ground"))
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            isExploded = true;
            playExplodeSound();
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("explode");
            DestroySelf(0f);
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
        target.TakeDamage(transform, damage, knockbackForce, damageType);
    }
    
}
