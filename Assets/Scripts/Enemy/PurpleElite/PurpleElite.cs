using UnityEngine;

public class PurpleElite : Enemy
{
    public PurpleEliteIdleState idleState;
    new public PurpleElitePatrolState patrolState;
    new public PurpleEliteChaseState chaseState;
    public PurpleEliteMeleeState meleeState;
    public PurpleEliteSpellState spellState;

    [SerializeField] private string debugCurrentState;
    [Header("Melee Settings")]
    public float meleeRange = 1.5f;               
    public float meleeCooldown = 2f;             
    public float meleeTimer;    

    [Header("Spell Settings")]
    public float spellRange = 5f;                 
    public float spellCooldown = 3f;             
    public float spellTimer;    
    public GameObject lightningPrefab;           

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

    protected override void Awake()
    {
        base.Awake();

        idleState = new PurpleEliteIdleState();
        patrolState = new PurpleElitePatrolState();
        chaseState = new PurpleEliteChaseState();
        meleeState = new PurpleEliteMeleeState();
        spellState = new PurpleEliteSpellState();

        currentState = patrolState;
    }
    //protected override void HandleOnTakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    //{
    //    if (isAttacking) return;
       

    //}
    protected override void OnEnable()
    {
        base.OnEnable();
        SwitchState(patrolState);
    }
    protected override void Start()
    {
        base.Start();
        SwitchState(patrolState);
    }
    protected override void Update()
    {
        base.Update();
        meleeTimer -= Time.deltaTime;
        spellTimer -= Time.deltaTime;
    }


    //public override void SwitchState(EnemyState state)
    //{
    //    if (isDead) return;
    //    Debug.Log("[PurpleElite] Switching state to: " + state);
    //    var newState = state switch
    //    {
    //        EnemyState.Patrol => patrolState,
    //        EnemyState.Chase => chaseState,
    //        EnemyState.Attack => attackState,
    //        _ => null
    //    };
    //    if (newState != null)
    //    {
    //        currentState.OnExit();
    //        currentState = newState;
    //        currentState.OnEnter(this);
    //    }
    //    else
    //    {
    //        Debug.LogError("Invalid state: " + state);
    //    }
    //}

    public override void SwitchState(BaseState newState)
    {
        if (isDead) return;

        Debug.Log($"[PurpleElite] Switching to {newState.GetType().Name}");
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

    public bool IsInSpellRange()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player instance is null");
            return false;
        }
        return Vector2.Distance(transform.position, Player.Instance.transform.position) <= spellRange;
    }

    public bool IsMeleeReady()
    {
        return meleeTimer <= 0f;
    }

    public bool IsSpellReady()
    {
        return spellTimer <= 0f;
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

        // Get 2D bounds
        Bounds bounds = chaseZoneCollider.bounds;

        // Get 2D position of player
        Vector2 playerPos2D = Player.Instance.transform.position;

        // Compare manually only x/y
        return (playerPos2D.x >= bounds.min.x && playerPos2D.x <= bounds.max.x &&
                playerPos2D.y >= bounds.min.y && playerPos2D.y <= bounds.max.y);
    }

    public bool CanSeePlayer()
    {
        Vector2 boxSize = sightBoxSize; 
        Vector2 origin = (Vector2)rayCast.position + (Vector2)faceDir.normalized * (boxSize.x / 2f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.zero, 0f, raycastMask);

        Debug.DrawLine(origin - new Vector2(boxSize.x / 2f, 0), origin + new Vector2(boxSize.x / 2f, 0), Color.cyan); 
        Debug.DrawLine(origin - new Vector2(0, boxSize.y / 2f), origin + new Vector2(0, boxSize.y / 2f), Color.cyan); 

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
    public void SpawnLightningAtPlayer()
    {
        if (Player.Instance == null) return;

        Vector3 targetPos = Player.Instance.transform.position+new Vector3(0,5,0);
        GameObject lightning = GameObject.Instantiate(lightningPrefab, targetPos, Quaternion.identity);
        Debug.Log("[SpellState] Lightning spawned at player position");
    }

    public override void Die()
    {
        if (isDead) return;
        anim.SetTrigger("dead");
        
        currentSpeed = 0;
        isDead = true;
        Debug.Log("[Purple Elite] has died.");
    }


}
