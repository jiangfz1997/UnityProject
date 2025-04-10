using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public EnemyPhysicsCheck physicsCheck;
    public Transform attacker;
    public CapsuleCollider2D coll;

    [Header("Movement")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    public int isFacingRight=1; // 1 for right, -1 for left

    [Header("Attack")]
    //public float hurtForce;
    public bool isChasing;
    public bool isAttacking;
    public float damageReduction;
    public bool isHurt;
    public bool isDead;
    public LayerMask attackLayer;
    private float originalSpeed;
    private Coroutine slowCoroutine;

    [Header("Counter")]
    public float waitTime;
    public float waitTimeCounter;
    public bool isWaiting;
    public float lostTime;
    public float lostTimeCounter;

    protected BaseState patrolState;
    protected BaseState currentState;
    protected BaseState chaseState;

    [Header("Detect")]
    public Vector2 centerDetectOffset;
    public Vector2 checkSize;
    public float checkDistance;

    [Header("Raycast Patrol")]
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public Transform leftLimit;
    public Transform rightLimit;
    public float patrolCooldown = 3f;

    [Header("Attack Config")]
    public float attackCooldown = 2f;
    public float attackCooldownCounter;
    public bool isAttackCoolingDown;
    public float attackDistance; 

    public RaycastHit2D hit;
    public Transform patrolTarget;
    public float lostTargetTimer;
    public MonsterSFX monsterSFX;
    public MonsterLoopSFXPlayer monsterLoopSFX;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<EnemyPhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();

        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;

    }

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<EnemyPhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();

        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;

        OnTakeDamage += HandleOnTakeDamage;
        DamageEffectHandler effectHandler = GetComponent<DamageEffectHandler>();
        if (effectHandler == null)
        {
            effectHandler = gameObject.AddComponent<DamageEffectHandler>();
        }
        effectHandler.Initialize(GetComponent<SpriteRenderer>());

      

        this.damageHandler = new DamageHandler(this, effectHandler);
        buffSystem = GetComponent<BuffSystem>();
        buffSystem.Initialize(this, effectHandler);
        originalSpeed = currentSpeed;

        if (leftLimit == null || rightLimit == null)
        {
            leftLimit = FindClosestLimit("LeftLimit");
            rightLimit = FindClosestLimit("RightLimit");
        }
        SelectPatrolTarget();

        if (monsterLoopSFX != null) monsterLoopSFX.StartAllLoopSounds();
    }
    protected virtual void OnEnable() { }
    protected override void Update()
    {
        if (isDead) return;
        
        base.Update();
        faceDir = new Vector3(transform.localScale.x*isFacingRight, 0, 0);
        float modelWidth = coll.bounds.extents.x * 2;
        centerDetectOffset = new Vector2(faceDir.x * (modelWidth / 2 + checkDistance / 2), centerDetectOffset.y);
        if (currentState != null) 
        { 
        
            currentState.LogicUpdate();
        }
        UpdateAttackCooldown();
        TimeCounter();
        DebugRay();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead)
            Move();
        if (currentState != null) 
        { 
        
            currentState.PhysicsUpdate();
        }
    }

    private void OnDisable()
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
    }
    private void UpdateAttackCooldown()
    {
        if (isAttackCoolingDown)
        {
            attackCooldownCounter -= Time.deltaTime;
            if (attackCooldownCounter <= 0f)
            {
                isAttackCoolingDown = false;
            }
        }
    }
    public void SetAttackCoolDown()
    {
        isAttackCoolingDown = true;
        attackCooldownCounter = attackCooldown;
    }
    public virtual void Move()
    {
        rb.linearVelocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.linearVelocity.y);
    }

    public void TimeCounter()
    {
        if (isWaiting)
        {
            waitTimeCounter -= Time.deltaTime;
            anim.SetBool("walk", false);
            if (waitTimeCounter <= 0)
            {
                isWaiting = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                faceDir = new Vector3(-transform.localScale.x, 0, 0);
                currentSpeed = normalSpeed;
                anim.SetBool("walk", true);
            }
        }

        if (!FoundPlayer())
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else
        {
            lostTimeCounter = lostTime;
        }
    }

    public override void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        if (attacker == null || !attacker.gameObject.CompareTag("PlayerAttack"))
        {
            return;
        }

        if (isInvincible||isDead) return;

        try
        {
            damageHandler.HandleDamage(attacker, damage, knockbackForce, damageType);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            Debug.LogError("Error in handling damage");
        }

        if (currentHP <= 0)
        {
            OnDie?.Invoke(transform);
        }
        else
        {
            TriggerInvincible(invincibleTime);
            InvokeTakeDamageEvent(attacker, damage, knockbackForce, damageType);
        }

        OnHealthChange?.Invoke(this);
    }

    private IEnumerator OnHurt(Vector2 repelDir, float knockbackForce)
    {
        
        //Debug.Log($"OnHurt add force {knockbackForce} + {repelDir * (knockbackForce - damageReduction)}");
        //rb.AddForce(repelDir * (knockbackForce - damageReduction), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    private void HandleOnTakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        rb.linearVelocity = Vector2.zero;
        Vector2 repelDir = new Vector2(0,0);
        StartCoroutine(OnHurt(repelDir, knockbackForce));
        isHurt = true;
        anim.SetTrigger("hurt");
        monsterSFX.PlayHurtSound();

    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    public void DestoryCollider()
    {
        coll.enabled = false;
    }

    public virtual bool FoundPlayer()
    {
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public virtual void SwitchState(EnemyState state) { }

    // -------------------- 附加方法：射线检测与限位巡逻 --------------------
    public bool IsPlayerInSight()
    {
        hit = Physics2D.Raycast(rayCast.position, faceDir, rayCastLength, raycastMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public void DebugRay()
    {
        Color color = IsPlayerInSight() ? Color.green : Color.red;
        Debug.DrawRay(rayCast.position, faceDir * rayCastLength, color);
    }

    public bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    //public void SelectPatrolTarget()
    //{
    //    float distToLeft = Vector2.Distance(transform.position, leftLimit.position);
    //    float distToRight = Vector2.Distance(transform.position, rightLimit.position);
    //    patrolTarget = (distToLeft > distToRight) ? leftLimit : rightLimit;
    //    FlipToTarget(patrolTarget);
    //}

    public Vector2 GetFacingDirection()
    {
        return new Vector2(-transform.localScale.x, 0);
    }
    public void SelectPatrolTarget()
    {
        bool leftSafe = !(physicsCheck.touchLeftWall || physicsCheck.isCliffAhead && GetFacingDirection().x > 0);
        bool rightSafe = !(physicsCheck.touchRightWall || physicsCheck.isCliffAhead && GetFacingDirection().x < 0);

        float distToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distToRight = Vector2.Distance(transform.position, rightLimit.position);

        // 优先选择安全方向中距离远的
        if (leftSafe && rightSafe)
        {
            patrolTarget = (distToLeft > distToRight) ? leftLimit : rightLimit;
        }
        else if (leftSafe)
        {
            patrolTarget = leftLimit;
        }
        else if (rightSafe)
        {
            patrolTarget = rightLimit;
        }
        else
        {
            // ❗ 两边都不安全（撞墙或悬崖）——原地停下
            patrolTarget = transform;
            Debug.LogWarning("两边都不安全，敌人停止巡逻！");
            return;
        }

        FlipToTarget(patrolTarget);
    }

    public void FlipToTarget(Transform target)
    {
        Vector3 scale = transform.localScale;
        scale.x = (transform.position.x > target.position.x) ? -1 : 1;
        transform.localScale = scale;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 start = transform.position + (Vector3)centerDetectOffset;
        Vector3 end = start + (Vector3)faceDir * checkDistance;

        Gizmos.DrawWireCube(start, checkSize);
        Gizmos.DrawWireCube(end, checkSize);
        Gizmos.DrawLine(start, end);
    }

    public override void ModifyHP(float amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
    }

    private Transform FindClosestLimit(string tag)
    {
        GameObject[] limits = GameObject.FindGameObjectsWithTag(tag);
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject limit in limits)
        {
            float distance = Vector2.Distance(transform.position, limit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = limit.transform;
            }
        }
        return closest;
    }

    public virtual void MoveTo(Vector2 targetPosition)
    {
        if (isHurt || isDead || isAttacking) return;
        Vector2 currentPosition = transform.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;
        rb.linearVelocity = new Vector2(direction.x * currentSpeed, rb.linearVelocityY);

        // 自动翻转朝向
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }

        //anim.SetBool("walk", true);
    }

    public void ApplySlow(float percent, float duration)
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowRoutine(percent, duration));
    }

    private IEnumerator SlowRoutine(float percent, float duration)
    {
        currentSpeed = originalSpeed * (1f - percent);
        yield return new WaitForSeconds(duration);
        currentSpeed = originalSpeed;
    }
}
