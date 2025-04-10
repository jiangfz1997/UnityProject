using UnityEngine;

public class LaserDamager : MonoBehaviour
{
    public Transform bossTransform;
    private float damageAmount = 20;
    [SerializeField] private float knockbackCooldown = 1f;
    [SerializeField] private float knockbackTimer = 0f;
    [SerializeField] private float forceAmount = 10f;
    private bool isDamaging = false;

    // public void Start()
    // {
    //     Debug.Log("LaserDamager Start");
    // }

    public void Initialize(float damage)
    {
        damageAmount = damage;
        isDamaging = true;
        bossTransform = GameObject.FindGameObjectWithTag("Boss1").transform;
    }

    private void Update()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isDamaging) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Damaged by Laser");
            collision.GetComponent<Player>().TakeDamage(bossTransform, damageAmount, 10f, DamageType.Physical);

            // Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            
            // if (playerRb != null && knockbackTimer <= 0)
            // {
            //     knockbackTimer = knockbackCooldown;

            //     playerRb.linearVelocityY = 8f;

            //     int dir = playerRb.linearVelocityX > 0 ? -1 : 1;

            //     Vector2 forceDirection = (3 * dir * transform.right + transform.up * 5).normalized;
            //     Debug.Log("forceDirection"+forceDirection);

            //     playerRb.AddForce(forceDirection * forceAmount, ForceMode2D.Impulse);

            // }

        }
    }
}