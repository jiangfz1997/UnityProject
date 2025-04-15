using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Basic")]
    public float speed = 1f;
    public float damage = 1f;
    public float lifeTime = 2f;
    public float knockbackForce = 10f;
    public Vector2 direction;
    public DamageType damageType;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioClip flyingSFX;
    public AudioClip explodeSFX;
    protected AudioSource audioSource;
    protected bool isExploded = false;

    public virtual void Initialize(Vector2 shootDirection, ProjectileData data)
    {
        direction = shootDirection.normalized;
        speed = data.speed;
        damage = data.damage;
        knockbackForce = data.knockbackForce;
        damageType = data.damageType;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * speed;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteRenderer.flipX = (direction.x < 0);

        audioSource = GetComponent<AudioSource>();
        if (flyingSFX != null)
        {
            audioSource.clip = flyingSFX;
            audioSource.loop = true;
            audioSource.Play();
        }

        Destroy(gameObject, lifeTime);
    }
    protected virtual void OnHit(Character target) { }
    public virtual void DestroySelf(float time=0.5f)
    {
        Destroy(gameObject, time);
    }
    protected abstract void OnTriggerEnter2D(Collider2D collision);

}
