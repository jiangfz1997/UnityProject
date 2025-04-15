using UnityEngine;

public class Slim : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BaseState attackState;
    public float dropChance = 0.5f; 
    protected override void Awake()
    {
        base.Awake();

        patrolState = new SlimIdleState();
        chaseState = new SlimChaseState();
        attackState = new SlimAttackState();
        currentState = patrolState;
    }
    protected override void Start()
    {
        base.Start();
        SwitchState(EnemyState.Patrol);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    public override void SwitchState(EnemyState state)
    {
        if (isDead) return;
        Debug.Log("[Slim] Switching state to: " + state);
        var newState = state switch
        {
            EnemyState.Patrol => patrolState,
            EnemyState.Chase => chaseState,
            EnemyState.Attack => attackState,
            _ => null
        };
        if (newState != null)
        {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter(this);
        }
        else
        {
            Debug.LogError("Invalid state: " + state);
        }
    }

    public void GenerateLoot()
    {
        // 生成掉落物品
        if (Random.value < dropChance)
        {
            GoldGenerator.Instance.GenerateGolds(transform.position, 100);
        }
    }

    public override void Die()
    {
        if (isDead) return;
        anim.SetTrigger("dead");
        currentSpeed = 0;
        isDead = true;
        Debug.Log("Slim died.");
    }

    //public void OnDestroy()
    //{
    //    Destroy(this.gameObject);
    //}
}
