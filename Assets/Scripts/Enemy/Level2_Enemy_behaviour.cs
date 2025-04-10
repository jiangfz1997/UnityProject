using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_behaviour : Character
{
    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;

    public int maxHealth = 100;
    public float damageReduction;
    //public float invincibleTime;
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private Transform target;
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool attackMode;
    private bool inRange; //Check if Player is in range
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;

    //public int currentHealth;
    private float patrolCooldown = 3f; // 丢失目标后恢复巡逻的时间
    private float lostTargetTimer = 0f;
    #endregion

    void Awake()
    {
        SelectTarget();
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();
        //currentHealth = maxHealth;
        currentHP = maxHP;
    }

    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        //When Player is detected
        if (hit.collider != null)
        {
            EnemyLogic();
            //
            lostTargetTimer = 0f; // 重置丢失目标计时器
        }
        else if (hit.collider == null && inRange)
        {
            lostTargetTimer += Time.deltaTime; // 开始计时
            if (lostTargetTimer >= patrolCooldown)
            {
                Debug.Log("玩家丢失，恢复巡逻");
                inRange = false; // 让敌人退出战斗模式
                StopAttack();
                SelectTarget(); // 重新选择巡逻目标
            }
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (inRange == false)
        {
            StopAttack();
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        //Debug.Log("玩家进入探测范围！！！");
        if (trig.gameObject.tag == "Player")
        {
            target = trig.transform;
            inRange = true;
            Flip();
        }
    }

    //
    void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            //Debug.Log("玩家离开探测范围，恢复巡逻");
            inRange = false; // 敌人停止追踪玩家
            StopAttack();
            SelectTarget(); // 重新选择巡逻目标
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        anim.SetBool("canWalk", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            //Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            //transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (!inRange) // 如果玩家不在范围内，回到巡逻模式
            {
                Vector2 patrolTarget = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, patrolTarget, moveSpeed * Time.deltaTime);

                // 如果已经靠近巡逻目标点，切换方向
                if (Vector2.Distance(transform.position, target.position) < 0.2f)
                {
                    SelectTarget();
                }
            }
            else // 玩家仍然在范围内，继续追踪
            {
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    void Attack()
    {
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackMode = true; //To check if Enemy can still attack or not

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideOfLimits()
    {
        //return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
        Vector3 leftWorldPos = leftLimit.position;
        Vector3 rightWorldPos = rightLimit.position;
        return transform.position.x > leftWorldPos.x && transform.position.x < rightWorldPos.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        //Ternary Operator
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Flip();
    }

    void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180;
        }
        else
        {
            Debug.Log("Twist");
            rotation.y = 0;
        }

        //Ternary Operator
        //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

        transform.eulerAngles = rotation;
    }

    //void TakeDamage(int damage)
    //{
    //    currentHealth -= damage; // 受到伤害
    //    anim.SetTrigger("hurt"); // 播放受伤动画

    //    if (currentHealth <= 0)
    //    {
    //        Die();
    //    }
    //}
     public override void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        Debug.Log("TakeDamage() - attacker: {attacker.name}, Tag: {attacker.tag}");
        // Only take damage from player attack (not include collision with player)
        if (attacker.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.Log("Take Damage 受到玩家攻击");
            Debug.Log("受到攻击！当前 HP: {currentHP}");
            anim.SetTrigger("hurt"); // 播放受伤动画
        }
        if (attacker == null || !attacker.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.LogError("TakeDamage() - attacker 为空！");
            return;
        }


        //if (isInvincible)
        //{
        //    Debug.Log("敌人无敌状态，攻击无效！");
        //    return;
        //}

        float finalDamage = damage * (1 - damageReduction);
        currentHP = Mathf.Max(currentHP - finalDamage, 0);
        Debug.Log("!!!!!!当前血量currentHP="+currentHP);
        if (currentHP <= 0)
        {
            OnDie?.Invoke(transform);
            Die();
        }
        else
        {
            //TriggerInvincible(invincibleTime);
            InvokeTakeDamageEvent(attacker, damage, knockbackForce, damageType);
        }

        OnHealthChange?.Invoke(this);
    }
    void Die()
    {
        Debug.Log("Enemy Died!");

        anim.SetTrigger("death"); 
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false; 

        Destroy(gameObject, 2f);
    }

    protected override void Start()
    {
        if (leftLimit == null || rightLimit == null)
        {
            leftLimit = FindClosestLimit("LeftLimit");
            rightLimit = FindClosestLimit("RightLimit");
        }
    }

    Transform FindClosestLimit(string tag)
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

    public override void ModifyHP(float amount)
    {
        throw new System.NotImplementedException();
    }
}
