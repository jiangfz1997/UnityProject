using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    public Rigidbody2D rb;
    //[HideInInspector] public Animator anim;
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

    [SerializeField] protected Vector2 sightBoxSize = new Vector2(2f, 2f);

    [SerializeField] private BaseState debugCurrentState;

    private FlashEffect flashEffect;
    private bool hasPlayedDeathAnim = false;





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
        //SelectPatrolTarget();

        if (monsterLoopSFX != null) monsterLoopSFX.StartAllLoopSounds();

        flashEffect = GetComponent<FlashEffect>();
    }
    protected virtual void OnEnable() { }
    protected override void Update()
    {
        if (isDead)
        {
            if (!hasPlayedDeathAnim)
            {
                if (anim != null)
                {
                    anim.speed = 1f;               // ✅ 保证能动
                    anim.SetTrigger("dead");       // ✅ 播一次就行
                }

                hasPlayedDeathAnim = true;
            }

        }

        base.Update();

        

        faceDir = new Vector3(transform.localScale.x*isFacingRight, 0, 0);
        float modelWidth = coll.bounds.extents.x * 2;
        centerDetectOffset = new Vector2(faceDir.x * (modelWidth / 2 + checkDistance / 2), centerDetectOffset.y);
        if (currentState != null) 
        {
            debugCurrentState = currentState;
            currentState.LogicUpdate();
        }
        UpdateAttackCooldown();
        TimeCounter();
        DebugRay();
    }

    protected virtual void FixedUpdate()
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

    protected virtual void HandleOnTakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        if (isAttacking) return;
        rb.linearVelocity = Vector2.zero;
        Vector2 repelDir = new Vector2(0,0);
        //StartCoroutine(OnHurt(repelDir, knockbackForce));
        //isHurt = true;
        anim.SetTrigger("hurt");
        //monsterSFX.PlayHurtSound();
        StartCoroutine(HurtPauseCoroutine(attacker));


    }

    protected virtual IEnumerator HurtPauseCoroutine(Transform attacker)
    {
        isHurt = true; 

        float freezeDuration = 1f;

       
        if (attacker != null)
        {
            Vector3 dir = attacker.position - transform.position;
            if (dir.x != 0)
                transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
        }

        yield return new WaitForSeconds(freezeDuration);

        isHurt = false;
        MoveTo(attacker.position); 

    }
    public void Flip()
    {
        //Debug.Log("Doing Flip");
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
    public virtual void SwitchState(BaseState state) { }


    public bool IsPlayerInSight()
    {
        Vector2 boxSize = sightBoxSize;
        Vector2 offset = faceDir.normalized * (boxSize.x / 2f);
        Vector2 boxCenter = (Vector2)rayCast.position + offset;

       
        hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.zero, 0f, raycastMask);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    //private void DebugDrawBox(Vector2 center, Vector2 size, Color color)
    //{
    //    Vector2 halfSize = size / 2f;

    //    // 计算盒子的四个角
    //    Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
    //    Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
    //    Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
    //    Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);

    //    // 绘制盒子边框
    //    Debug.DrawLine(topLeft, topRight, color, 0.1f);
    //    Debug.DrawLine(topRight, bottomRight, color, 0.1f);
    //    Debug.DrawLine(bottomRight, bottomLeft, color, 0.1f);
    //    Debug.DrawLine(bottomLeft, topLeft, color, 0.1f);
    //}

    public void DebugRay()
    {
       
        //Debug.DrawRay(rayCast.position, faceDir * rayCastLength, color);
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
        return new Vector2(transform.localScale.x*isFacingRight, 0);
    }
    public void SelectPatrolTarget()
    {
        bool leftSafe = !(physicsCheck.touchLeftWall || physicsCheck.isCliffAhead && GetFacingDirection().x > 0);
        bool rightSafe = !(physicsCheck.touchRightWall || physicsCheck.isCliffAhead && GetFacingDirection().x < 0);

        float distToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distToRight = Vector2.Distance(transform.position, rightLimit.position);

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
            patrolTarget = transform;
            //Debug.LogWarning($"两边都不安全，敌人停止巡逻！left:{physicsCheck.touchLeftWall}right:{physicsCheck.touchRightWall}, cliff:{physicsCheck.isCliffAhead}");
            
            return;
        }
        //Debug.Log($"选择巡逻目标: {patrolTarget.name}");
        FlipToTarget(patrolTarget);
    }


    public void FreezeMovement()
    {
        currentSpeed = 0f;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
        Debug.Log($"FreezeMovement {currentSpeed} {rb.linearVelocity}");
    }


    public void FlipToTarget(Transform target)
    {
        float deltaX = target.position.x - transform.position.x;

        if (deltaX * faceDir.x < 0)
        {
            Flip();
        }
    }


    protected virtual void OnDrawGizmosSelected()
    {
        
        Vector2 boxSize = sightBoxSize;

        Vector2 offset = faceDir.normalized * (boxSize.x / 2f);
        Vector2 boxCenter = (Vector2)rayCast.position + offset;
        Color color = IsPlayerInSight() ? Color.green : Color.red;
        Gizmos.color = color;
        //Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rayCast.position, 0.1f); 

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(boxCenter, 0.1f); 
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

    public void SetDefence(float defence) { this.defence = defence; }
    
}
