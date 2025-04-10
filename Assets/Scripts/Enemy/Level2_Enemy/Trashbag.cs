using UnityEngine;

public class Trashbag : Enemy
{
    public BaseState attackState;
    protected override void Awake()
    {
        base.Awake();

        // ��ʼ�� Trashbag ��״̬��
        patrolState = new TrashbagPatrolState();
        chaseState = new TrashbagChaseState();
        attackState = new TrashbagAttackState();
        currentState = patrolState;
    }

    protected override void Start()
    {
        base.Start();
        // Ĭ�Ͻ���Ѳ��״̬
        SwitchState(EnemyState.Patrol);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // ���¼���ʱ����Ϊ��ǰ״̬��һ��ΪѲ�ߣ�
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    //public override bool FoundPlayer()
    //{
    //    // ʹ�û����װ�õ����߼��
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

