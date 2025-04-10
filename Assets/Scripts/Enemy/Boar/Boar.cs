using UnityEngine;

public class Boar : Enemy
{

    public int goldDrop = 10;
    //public GoldGenerator goldGenerator;
    override public void Move()
    {
        base.Move();
        anim.SetBool("walk", true);
    }

    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
        Debug.Log("Boar Patrol State");

        // ✅ 确保 `GoldGenerator` 存在
        //goldGenerator = gameObject.GetComponent<GoldGenerator>();
        //if (goldGenerator == null)
        //{
        //    goldGenerator = gameObject.AddComponent<GoldGenerator>();
        //}
    }

    protected override void Start()
    {
        //goldGenerator = gameObject.AddComponent<GoldGenerator>();
        Debug.Log("Boar Start");
        base.Start();
        SwitchState(EnemyState.Patrol);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    public override bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerDetectOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }

    protected override void Update()
    {
        
        base.Update();
        
    }

    public override void SwitchState(EnemyState state)
    {
        Debug.Log("Switch State to: "+state);
        var newState = state switch
        {
            EnemyState.Patrol => patrolState,
            EnemyState.Chase => chaseState,
            _ => null
        };

        if (newState != null)
        {
            Debug.Log("Enter state: " + newState);
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter(this);
        }
        else
        {
            Debug.LogError("Invalid State"+newState);
        }
    }
    public override void Die()
    {
        //base.Die();
        //GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity);
        //gold.GetComponent<Gold>().Initialize();
        if (isDead) return;
        anim.SetTrigger("dead");
        isDead = true;
        Debug.Log("Boar die");
        
    }

    public void GenerateLoot()
    {

        GoldGenerator.Instance.GenerateGolds(transform.position, 100);

    }
}
