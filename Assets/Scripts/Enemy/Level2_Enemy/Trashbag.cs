using UnityEngine;

public class Trashbag : Enemy
{
    public BaseState attackState;
    protected override void Awake()
    {
        base.Awake();

        // 初始化 Trashbag 的状态类
        patrolState = new TrashbagPatrolState();
        chaseState = new TrashbagChaseState();
        attackState = new TrashbagAttackState();
        currentState = patrolState;
    }

    protected override void Start()
    {
        base.Start();
        // 默认进入巡逻状态
        SwitchState(EnemyState.Patrol);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // 重新激活时重置为当前状态（一般为巡逻）
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    //public override bool FoundPlayer()
    //{
    //    // 使用基类封装好的射线检测
    //    return IsPlayerInSight();
    //}

    public override void SwitchState(EnemyState state)
    {
        if (isDead) return;
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
        GoldGenerator.Instance.GenerateGolds(transform.position, 200);
    }
    public override void Die()
    {
        if (isDead) return;
        anim.SetTrigger("dead");
        currentSpeed = 0;
        isDead = true;
        Debug.Log("Trashbag died.");
    }
}

