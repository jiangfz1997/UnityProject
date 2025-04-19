using UnityEngine;

public class MirrorLaser : MonoBehaviour
{
    private BoxCollider2D collider;
    private Transform bossTransform;
    [SerializeField] private float damage = 75f;

    [SerializeField] private float coefficient = 1f;

    public void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        bossTransform = GameObject.FindGameObjectWithTag("Boss2").transform;
        collider.enabled = true;

        if (CollectionManager.Instance.IsEffectActivated(3))
            coefficient *= 0.95f;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player hit by mirror laser");
            other.GetComponent<Player>().TakeDamage(bossTransform, damage*coefficient, 2f, DamageType.Physical);
        }
    }

    public void RemoveCollider()
    {
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    public void DestroyLaser()
    {
        Destroy(gameObject);
    }
    
}
