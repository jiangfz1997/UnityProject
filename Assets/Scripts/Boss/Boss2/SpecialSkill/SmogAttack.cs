using UnityEngine;

public class SmogAttack : MonoBehaviour
{
    private Transform bossTransform;
    private Transform transform;
    private Animator animator;

    private BoxCollider2D smogCollider;
    [SerializeField] private float[] smogLengths;
    [SerializeField] private float[] smogHeights;
    [SerializeField] private Vector2 offset;

    [SerializeField] private int damage = 75;
    [SerializeField] private float coefficient = 1f;

    
    void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        smogCollider = GetComponent<BoxCollider2D>();
        bossTransform = GameObject.FindGameObjectWithTag("Boss2").transform;

        if (CollectionManager.Instance.IsEffectActivated(3))
            coefficient *= 0.95f;

        smogCollider.enabled = false;
    }
    public void StartSmogAttack()
    {
        AdjustDirection();
        smogCollider.enabled = true;
        UpdateColliderShape(0);
    }

    private void AdjustDirection()
    {
        bool facingRight = bossTransform.localScale.x > 0;
        
        if (facingRight)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    public void EndSmogAttack()
    {
        smogCollider.enabled = false;
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player hit by smog attack");
            other.GetComponent<Player>().TakeDamage(bossTransform, damage*coefficient, 2f, DamageType.Physical);
        }
    }
    
    public void UpdateColliderShape(int keyframeIndex)
    {
        // Debug.Log("UpdateColliderShape called with keyframeIndex: " + keyframeIndex);
        if (keyframeIndex >= 0 && keyframeIndex < smogLengths.Length)
        {
            float currentLength = smogLengths[keyframeIndex];
            float currentHeight = smogHeights[keyframeIndex];

            smogCollider.size = new Vector2(currentLength, currentHeight);
            
            bool facingRight = transform.localScale.x > 0;
            
            if (facingRight)
            {
                smogCollider.offset = new Vector2(
                    offset.x, offset.y
                );
            }
            else
            {
                smogCollider.offset = new Vector2(
                    -offset.x, offset.y
                );
            }

            smogCollider.enabled = true;
        }
    }

    // void OnDrawGizmos()
    // {
    //     if (smogCollider != null && smogCollider.enabled)
    //     {
    //         Gizmos.color = Color.red;

    //         Vector2 worldPos = transform.position + (Vector3)smogCollider.offset;
    //         Vector2 worldSize = smogCollider.size;

    //         Gizmos.DrawWireCube(worldPos, worldSize);
    //     }
    // }
}

