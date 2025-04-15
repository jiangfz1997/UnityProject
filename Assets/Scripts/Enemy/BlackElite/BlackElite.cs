using System.Collections;
using UnityEngine;

public class BlackElite : Enemy
{
    //public BlackEliteIdleState idleState;
    new public BlackElitePatrolState patrolState;
    new public BlackEliteChaseState chaseState;
    public BlackEliteMeleeState meleeState;
    public BlackEliteShootState shootState;
    public BlackEliteDashAttackState dashAttackState;

    [SerializeField] private string debugCurrentState;
    [Header("Melee Settings")]
    public float meleeRange = 3f;
    public float meleeCooldown = 2f;
    public float meleeTimer;

    [Header("Shoot Settings")]
    public float shootRange = 10f;
    public float shootCooldown = 3f;
    public float shootTimer;
    public GameObject shootLaserPrefab;
    public Transform shootOrigin;
    public float shootChance = 0.5f;
    public int shootCount = 3;
    public float shootDelay = 0.2f;
    public float shootOffsetY = -0.5f;
    public float shootOffsetX = 0.5f;
    public Vector2 shootDirection = Vector2.left + Vector2.up * 0.2f;

    [Header("Dash Setting")]
    public float dashRange = 6f;
    public float dashCooldown = 5f;
    public float dashDistance = 4f;
    public float dashDuration = 0.2f;
    public float dashTimer;


    [Header("Vision Settings")]
    public float maxChaseDistance = 10f;
    //public Transform raycastOrigin;              
    //public LayerMask visionMask;
    //
    [Header("Chase Zone Settings")]
    public bool isTrackingPlayer = false;
    public Collider2D chaseZoneCollider;

    [Header("Chase Settings")]
    public float chaseSpeedMultiplier = 1.5f;
    public float chaseAnimSpeedMultiplier = 1.5f;

    [Header("Other Settings")]
    //public Transform firePoint;                  
    public float dropChance = 1f;
    [SerializeField] private Vector2 sightBoxSize = new Vector2(2f, 2f);
    public Vector2 sightBoxOffset = new Vector2(0f, 0f);

    private Coroutine dashRoutine;

    public bool isSheilded = false;
    protected override void Awake()
    {
        base.Awake();
        //idleState = new BlackEliteIdleState();
        patrolState = new BlackElitePatrolState();
        chaseState = new BlackEliteChaseState();
        meleeState = new BlackEliteMeleeState();
        shootState = new BlackEliteShootState();
        dashAttackState = new BlackEliteDashAttackState();
        currentState = patrolState;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnEnable()
    {
        base.OnEnable();
        if (currentState == null)
        {
            currentState = patrolState;
        }
    }
    protected override void Start()
    {
        base.Start();
        SwitchState(patrolState);
    }
    protected override void Update()
    {
        base.Update();
        debugCurrentState = currentState.ToString();
        meleeTimer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;

        if (isDead) return;




    }


    public override void SwitchState(BaseState newState)
    {
        if (isDead) return;

        Debug.Log($"[BlackElite] Switching to {newState.GetType().Name}");
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter(this);

        debugCurrentState = newState.GetType().Name;
    }

    public bool IsInMeleeRange()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player instance is null");
            return false;
        }
        return Vector2.Distance(transform.position, Player.Instance.transform.position) <= meleeRange;
    }
    public bool IsInShootRange()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player instance is null");
            return false;
        }
        return Vector2.Distance(transform.position, Player.Instance.transform.position) <= shootRange;
    }
    public bool IsInDashRange()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player instance is null");
            return false;
        }
        return Vector2.Distance(transform.position, Player.Instance.transform.position) <= dashRange;
    }
    public bool IsMeleeReady()
    {
        return meleeTimer <= 0f;
    }

    public bool IsShootReady()
    {
        return shootTimer <= 0f;
    }

    public bool IsDashReady()
    {
        return dashTimer <= 0f;
    }

    public void GenerateLoot()
    {
        if (Random.value < dropChance)
        {
            GoldGenerator.Instance.GenerateGolds(transform.position, 2000);
        }
        // Generate other loot here
    }

    public bool IsPlayerInChaseZone()
    {
        if (chaseZoneCollider == null || Player.Instance == null) return false;

        Bounds bounds = chaseZoneCollider.bounds;

        Vector2 playerPos2D = Player.Instance.transform.position;

        return (playerPos2D.x >= bounds.min.x && playerPos2D.x <= bounds.max.x &&
                playerPos2D.y >= bounds.min.y && playerPos2D.y <= bounds.max.y);
    }

    public bool CanSeePlayer()
    {
        Vector2 boxSize = sightBoxSize;
        Vector2 origin = (Vector2)rayCast.position + (Vector2)faceDir.normalized * (boxSize.x / 2f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.zero, 0f, raycastMask);

        Debug.DrawLine(origin - new Vector2(boxSize.x / 2f, 0), origin + new Vector2(boxSize.x / 2f, 0), Color.cyan); // Ë®Æ½
        Debug.DrawLine(origin - new Vector2(0, boxSize.y / 2f), origin + new Vector2(0, boxSize.y / 2f), Color.cyan); // ´¹Ö±

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public void SpawnLightningAtPlayer()
    {
        if (Player.Instance == null) return;

        //Vector3 targetPos = Player.Instance.transform.position + new Vector3(0, 5, 0);
        //GameObject lightning = GameObject.Instantiate(lightningPrefab, targetPos, Quaternion.identity);
        //Debug.Log("[SpellState] Lightning spawned at player position");
    }

    public override void Die()
    {
        if (isDead) return;
        anim.SetTrigger("dead");

        currentSpeed = 0;
        isDead = true;
        Debug.Log("[Black Elite] has died.");
    }


    public void TriggerDashMove()
    {
        //Vector3 direction = faceDir.x > 0 ? Vector3.right : Vector3.left;
        //transform.position += direction * dashDistance;

        //Debug.Log($"[Dash] Instantly dashed {dashDistance} units to the {(faceDir.x > 0 ? "right" : "left")}");
        if (dashRoutine != null)
        {
            StopCoroutine(dashRoutine);
        }
        dashRoutine = StartCoroutine(DoSmoothDash());

    }

    private IEnumerator DoSmoothDash()
    {
        Vector3 start = transform.position;
        Vector3 direction = faceDir.x > 0 ? Vector3.right : Vector3.left;
        Vector3 end = start + direction * dashDistance;

        float elapsed = 0f;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dashDuration);
            Vector3 nextPos = Vector3.Lerp(start, end, t);

            rb.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(end);
    }


    public void EndMeleeAttack()
    {
        if (currentState == meleeState)
        {
            meleeState.OnMeleeEnd();
        }
    }

    public void EndShootAttack()
    {
        if (currentState == shootState)
        {
            shootState.OnShootEnd();
        }
    }

    public void EndDashAttack()
    {
        if (currentState == dashAttackState)
        {
            dashAttackState.OnDashEnd();
        }
    }

    public void TriggerShootBurst()
    {
        StartCoroutine(ShootBurstCoroutine());
        shootTimer = shootCooldown;
    }

    private IEnumerator ShootBurstCoroutine()
    {
        for (int i = 0; i < shootCount; i++)
        {
            Vector3 offset = new Vector3(
                shootOffsetX * i,
                shootOffsetY * i,
                0f
            );

            Vector3 spawnPos = shootOrigin.position + offset;

            GameObject laser = Instantiate(shootLaserPrefab, spawnPos, Quaternion.identity);

            laser.transform.localScale = new Vector3(
                Mathf.Abs(laser.transform.localScale.x) * faceDir.x,
                laser.transform.localScale.y,
                laser.transform.localScale.z
            );

            yield return new WaitForSeconds(shootDelay);
        }

    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, dashRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shootRange);

        Vector2 boxSize = sightBoxSize;
        Vector2 origin = (Vector2)rayCast.position + (Vector2)faceDir.normalized * (boxSize.x / 2f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(origin, boxSize);

    }

    public override void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {

        if (isSheilded)
        {
            Debug.Log("[BlackElite] is shielded, no damage taken");
            return;
        }
        base.TakeDamage(attacker, damage, knockbackForce, damageType);

    }

    public void GenerateShielded()
    {
        isSheilded = true;
        Debug.Log("[BlackElite] is now shielded");    
    }
    public void RemoveShielded()
    {
        isSheilded = false;
        Debug.Log("[BlackElite] is no longer shielded");
    }
}
