using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Processors;

public class Enemy_old : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    [Header("Attack")]
    public float hurtForce;
    //public float invincibleTime;
    //public float chaseRange;
    //public float stopChaseRange;
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

    // -------------------- 新增字段：来自 Enemy_behaviour --------------------
    [Header("Raycast Patrol")]
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public Transform leftLimit;
    public Transform rightLimit;
    public float patrolCooldown = 3f;

    [Header("Attack Config")]
    public float attackCooldown = 2f;
    public float attackDistance = 1.5f;

    public RaycastHit2D hit;
    public Transform patrolTarget;
    public float lostTargetTimer;

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

        damageHandler = new DamageHandler(this, effectHandler);
        buffSystem = GetComponent<BuffSystem>();
        buffSystem.Initialize(this, effectHandler);
        originalSpeed = currentSpeed;

        // 如果没有设置巡逻点，尝试自动寻找
        //if (leftLimit == null || rightLimit == null)
        //{
        //    leftLimit = FindClosestLimit("LeftLimit");
        //    rightLimit = FindClosestLimit("RightLimit");
        //}
        //SelectPatrolTarget();
    }
    protected override void Update()
    {
        base.Update();
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        float modelWidth = coll.bounds.extents.x * 2;
        centerDetectOffset = new Vector2(faceDir.x * (modelWidth / 2 + checkDistance / 2), centerDetectOffset.y);
        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDead)
            Move();
        currentState.PhysicsUpdate();
    }

    protected virtual void OnEnable()
    {
      
    }
    public override void SetCurrentSpeed(float speed) { currentSpeed = speed; }

    public override float GetCurrentSpeed() { return currentSpeed; }

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

    public DamageType GetCurrentDamageType() => buffSystem.GetCurrentDamageType();
    public float GetAttackMultiplier() => buffSystem.GetAttackMultiplier();

    private void OnDisable()
    {
        currentState.OnExit();
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


        if (isInvincible) return;

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
        rb.AddForce(repelDir * (knockbackForce - damageReduction), ForceMode2D.Impulse);
        isHurt = true;
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }
    private void HandleOnTakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        Transform attackerTrans = attacker.transform;

        if (attackerTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackerTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        Vector2 repelDir = new Vector2(transform.position.x - attackerTrans.position.x, 0).normalized;
        StartCoroutine(OnHurt(repelDir, knockbackForce));
        // Get repelled
        isHurt = true;
        anim.SetTrigger("hurt");

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
        // Add logic to determine if the player is found
        return false; // Default return value
    }


    public virtual void SwitchState(EnemyState state){ }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
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

    //Transform FindClosestLimit(string tag)
    //{
    //    GameObject[] limits = GameObject.FindGameObjectsWithTag(tag); // 获取所有带这个 Tag 的对象
    //    Transform closest = null;
    //    float minDistance = Mathf.Infinity; // 初始设为无限大

    //    foreach (GameObject limit in limits)
    //    {
    //        float distance = Vector2.Distance(transform.position, limit.transform.position);
    //        if (distance < minDistance)
    //        {
    //            minDistance = distance;
    //            closest = limit.transform;
    //        }
    //    }
    //    return closest; // 返回最近的那个点
    //}

    public void SelectPatrolTarget()
    {
        float distToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distToRight = Vector2.Distance(transform.position, rightLimit.position);
        patrolTarget = (distToLeft > distToRight) ? leftLimit : rightLimit;
        FlipToTarget(patrolTarget);
    }

    public void FlipToTarget(Transform target)
    {
        Vector3 scale = transform.localScale;
        scale.x = (transform.position.x > target.position.x) ? -1 : 1;
        transform.localScale = scale;
    }

}

