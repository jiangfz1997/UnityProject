using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Basic")]
    public float speed = 1f;
    public float damage = 1f;
    public float lifeTime = 2f;
    public float knockbackForce = 10f;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public DamageType damageType;
    public Vector2 direction;
    public Animator animator;
    public bool isExploded = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(Vector2 shootDirection, ProjectileData data)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        direction = shootDirection;
        speed = data.speed;
        damage = data.damage;
        knockbackForce = data.knockbackForce;
        damageType = data.damageType;
        spriteRenderer.flipX = (direction.x < 0);
        rb.linearVelocity = direction * data.speed;
        Destroy(gameObject, 2f); // �Զ�����
    }

    protected virtual void Update()
    {
        if (isExploded) return;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploded) return;

        var character = collision.GetComponent<Character>();
        if (character != null && character.CompareTag("Enemy"))
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            isExploded = true;
            rb.linearVelocity = Vector2.zero;
            OnHit(character);

            //Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            isExploded = true;

            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("explode");
            //Destroy(gameObject);
        }
    }

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
    protected abstract void OnHit(Character target);
}
