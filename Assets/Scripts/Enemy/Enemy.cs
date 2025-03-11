using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Processors;

public class Enemy : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public EnemyPhysicsCheck physicsCheck;
    public Transform attacker;
    public CapsuleCollider2D coll;
    [Header("Movement")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;

    [Header("Attack")]
    public float hurtForce;
    public float invincibleTime;
    //public float chaseRange;
    //public float stopChaseRange;
    public bool isChasing;
    public bool isAttacking;
    public float damageReduction;
    public bool isHurt;
    public bool isDead;
    public LayerMask attackLayer;

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

    protected virtual void OnEnable()
    {

        

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

        // ¼àÌý `OnTakeDamage` ÊÂ¼þ
        OnTakeDamage += HandleOnTakeDamage;
    }



    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<EnemyPhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();

        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        float modelWidth = coll.bounds.extents.x * 2; 
        centerDetectOffset = new Vector2(faceDir.x * (modelWidth/2 + checkDistance / 2), centerDetectOffset.y);
        currentState.LogicUpdate();
        TimeCounter();
    }



    private void FixedUpdate()
    {
        if(!isHurt &&!isDead)
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    
    }

    public virtual void Move()
    {
        rb.linearVelocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.linearVelocity.y);
        
    }

    //<summary>
    //    Counter for the waiting time when the enemy hits the wall
    //</summary>
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
        // Only take damage from player attack (not include collision with player)
        if (attacker.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.Log("Take Damage");
        }
        if (attacker == null || !attacker.gameObject.CompareTag("PlayerAttack"))
        {
            return;
        }


        if (isInvincible) return;

        float finalDamage = damage * (1 - damageReduction);
        currentHP = Mathf.Max(currentHP - finalDamage, 0);

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

    //public new void OnTakeDamage(Transform attackTrans)
    //{
    //    attacker = attackTrans;

    //    if(attackTrans.position.x - transform.position.x>0)
    //        transform.localScale = new Vector3(-1, 1, 1);
    //    if (attackTrans.position.x - transform.position.x < 0)
    //        transform.localScale = new Vector3(1, 1, 1);
    //    Vector2 repelDir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
    //    StartCoroutine(OnHurt(repelDir));
    //    // Get repelled
    //    isHurt = true;
    //    anim.SetTrigger("hurt");
    //}

    //public override void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    //{
    //    if (isInvincible) return;

    //    currentHP = Mathf.Max(currentHP - damage, 0);
    //    if (currentHP <= 0)
    //    {
    //        OnDie?.Invoke(transform);
    //    }
    //    else
    //    {
    //        //TriggerInvincible();
    //        InvokeTakeDamageEvent(attacker, damage, knockbackForce, damageType);
    //    }

    //    OnHealthChange?.Invoke(this);
    //}
    private IEnumerator OnHurt(Vector2 repelDir, float knockbackForce)
    {
        rb.AddForce(repelDir * (knockbackForce - damageReduction), ForceMode2D.Impulse);
        isHurt = true;
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }
    private void HandleOnTakeDamage(Transform attackTrans, float damage, float knockbackForce, DamageType damageType)
    {


        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        Vector2 repelDir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
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


    public virtual void SwitchState(EnemyState state)
    {
       
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 start = transform.position + (Vector3)centerDetectOffset;
        Vector3 end = start + (Vector3)faceDir * checkDistance;

 
        Gizmos.DrawWireCube(start, checkSize);


        Gizmos.DrawWireCube(end, checkSize);

  
        Gizmos.DrawLine(start, end);
    }

    //public override void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    //{
    //    float finaldamage = damage * (1-damageReduction);
    //    currentHP = Mathf.Max(currentHP - finaldamage, 0);

    //    if (currentHP <= 0)
    //    {
    //        Die();
    //    }
    //    else
    //    {
    //        anim.SetTrigger("hurt");
    //    }
    //}
}

