using UnityEngine;

public class GreenGhost : Enemy
{
    public BaseState attackState;
    public float dropChance = 0.5f;
    public ProjectileDatabase projectileDatabase;
    public Transform firePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        patrolState = new GreenGhostPatrolState();
        chaseState = new GreenGhostChaseState();
        attackState = new GreenGhostAttackState();
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
        Debug.Log("[GreenGhost] Switching state to: " + state);
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
        if (Random.value < dropChance)
        {
            GoldGenerator.Instance.GenerateGolds(transform.position, 300);
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

    public void FireBullet()
    {
        string attackType = "GreenGhostProjectile";
        ProjectileData projectileData = projectileDatabase.GetProjectile(attackType);
        if (projectileData == null || firePoint == null) return;

        // Ensure Instantiate is properly recognized
        GameObject projectileInstance = Instantiate(projectileData.prefab, firePoint.position, Quaternion.identity);
        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        projectileInstance.GetComponent<BaseProjectile>().Initialize(shootDirection, projectileData);
    }
    

}
