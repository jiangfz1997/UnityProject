using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Basic")]
    public float speed = 10f;
    public float damage = 1f;
    public float lifeTime = 2f;
    public float knockbackForce = 10f;
    public Rigidbody2D rb;
    public DamageType damageType;
    public Vector2 direction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(Vector2 shootDirection, ProjectileData data)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        direction = shootDirection;
        speed = data.speed;
        damage = data.damage;
        knockbackForce = data.knockbackForce;
        damageType = data.damageType;

        rb.linearVelocity = direction * data.speed;
        Destroy(gameObject, 2f); // ×Ô¶¯Ïú»Ù
    }

    protected virtual void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null && character.CompareTag("Enemy"))
        {
            OnHit(character);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }


    protected abstract void OnHit(Character target);
}
