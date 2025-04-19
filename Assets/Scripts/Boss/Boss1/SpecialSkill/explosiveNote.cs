using System.Collections;
using UnityEngine;

public class explosiveNote : MonoBehaviour
{
    public Transform bossTransform;
    public float moveSpeed = 2f;
    public float lifetimeMax = 6f;
    public int damage = 20;
    public GameObject explosionEffectPrefab; 

    private float lifetime = 0f;
    private Vector2 moveDirection;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        moveDirection = Random.insideUnitCircle.normalized;

        spriteRenderer = GetComponent<SpriteRenderer>();

        bossTransform = GameObject.FindGameObjectWithTag("Boss1").transform;

        StartCoroutine(PulseAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // ���´��ʱ��
        lifetime += Time.deltaTime;
        if (lifetime >= lifetimeMax)
        {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Player"))
        {
            // Debug.Log("Player hit by explosive note");

            Explode();
            
            target.GetComponent<Player>().TakeDamage(bossTransform, damage, 10f, DamageType.Physical);

            
        }
    }

    void Explode()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    IEnumerator PulseAnimation()
    {
        while (true)
        {
            float time = 0;
            float duration = 0.5f;
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = startScale * Random.Range(0.8f, 1.1f);

            while (time < duration)
            {
                // ����
                transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);

                // ��ɫ����
                if (spriteRenderer != null)
                {
                    if (time < duration / 2)
                    {
                        spriteRenderer.color = Color.Lerp(Color.black, Color.red, time / (duration / 2));
                    }
                    else
                    {
                        spriteRenderer.color = Color.Lerp(Color.red, Color.black, (time - duration / 2) / (duration / 2));
                    }
                }

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
