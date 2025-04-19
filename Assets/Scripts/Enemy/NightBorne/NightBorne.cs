using UnityEngine;
using System.Collections;

public class NightBorne: Enemy
{
    public BaseState attackState;
    public float dropChance = 0.5f;
    protected override void Awake()
    {
        base.Awake();

        patrolState = new NightBornePatrolState();
        chaseState = new NightBorneChaseState();
        attackState = new NIghtBorneAttackState();
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
        // Éú³ÉµôÂäÎïÆ·
        if (Random.value < dropChance)
        {
            GoldGenerator.Instance.GenerateGolds(transform.position, 100);
        }
    }

    public override void Die()
    {
        if (isDead) return;
        if (anim != null)
            anim.speed = 2.2f;
        anim.SetTrigger("dead");
        currentSpeed = 0;
        isDead = true;
        Debug.Log("Wolf died.");

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2.2f); // 等待死亡动画播放完
        Destroy(gameObject); // 这里销毁自己
    }
}
