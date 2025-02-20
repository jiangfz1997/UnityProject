using UnityEngine;

public class Boar : Enemy
{


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

}
