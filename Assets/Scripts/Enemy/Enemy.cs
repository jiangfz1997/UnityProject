using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Rigidbody2D rb;
    protected Animator anim;
    protected PhysicsCheck physicsCheck;
    public Transform attacker;

    [Header("Movement")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;

    [Header("Attack")]
    public float attackSpeed;
    public float attackRange;
    public float chaseRange;
    public float stopChaseRange;
    public float attackRate;
    public float nextAttackTime;
    public float hurtForce;
    public bool isChasing;
    public bool isAttacking;
    public bool isHurt;


    [Header("Counter")]
    public float waitTime;
    public float waitTimeCounter;
    public bool isWaiting;
    void Start()
    {
        
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        TimeCounter();
        if ((physicsCheck.touchLeftWall && faceDir.x < 0) || (physicsCheck.touchRightWall && faceDir.x > 0))
        {
            isWaiting = true;
            anim.SetBool("walk", false);
        }
    }

    private void FixedUpdate()
    {
        if(!isHurt)
            Move();
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
            if (waitTimeCounter <= 0)
            {
                isWaiting = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                faceDir = new Vector3(-transform.localScale.x, 0, 0);
                anim.SetBool("walk", true);
            }
        }
    }

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;

        if(attackTrans.position.x - transform.position.x>0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        // Get repelled
        isHurt = true;
        anim.SetTrigger("Hurt");
        Vector2 repelDir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.AddForce(repelDir*hurtForce, ForceMode2D.Impulse);
    }
}
