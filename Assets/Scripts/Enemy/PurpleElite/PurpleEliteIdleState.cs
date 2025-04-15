using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PurpleEliteIdleState : BaseState
{
    private PurpleElite enemy;
    private float idleTimer;
    private float idleDuration;
    public override void LogicUpdate()
    {
        if (enemy == null || Player.Instance == null) return;

        // Check if player is seen
        if (enemy.CanSeePlayer())
        {
            enemy.SwitchState(enemy.chaseState);
            return;
        }

        // Count idle time
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            enemy.SwitchState(enemy.patrolState);
        }
    }

    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as PurpleElite;
        if (enemy == null)
        {
            Debug.LogError("[IdleState] Enemy is not PurpleElite!");
            return;
        }

        // Set idle time
        idleDuration = Random.Range(1.5f, 3f); 
        idleTimer = 0f;

        enemy.FreezeMovement();
        enemy.anim.SetBool("isIdle", true); 

        Debug.Log("[IdleState] Entered Idle");
    }

    public override void OnExit()
    {
        Debug.Log("[IdleState] Exit Idle");
        enemy.anim.SetBool("isIdle", false);
    }

    public override void PhysicsUpdate(){}

    

}
