using UnityEngine;

public class BlackElitePatrolState : BaseState
{
    private BlackElite enemy;

    public override void LogicUpdate()
    {
        if (enemy == null || Player.Instance == null) return;


        if (enemy.CanSeePlayer())
        {
            enemy.SwitchState(enemy.chaseState);
            return;
        }

        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.patrolTarget.position);
        if (distanceToTarget < 0.2f)
        {
            enemy.SelectPatrolTarget();
            return;
        }

        if (enemy.physicsCheck.isCliffAhead ||
            (enemy.physicsCheck.touchLeftWall && enemy.faceDir.x < 0) ||
            (enemy.physicsCheck.touchRightWall && enemy.faceDir.x > 0))
        {
            enemy.SelectPatrolTarget();
            return;
        }

        enemy.MoveTo(enemy.patrolTarget.position);
    }

    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as BlackElite;
        if (enemy == null)
        {
            Debug.LogError("[PatrolState] Enemy is not BlackElite!");
            return;
        }

        Debug.Log("[PatrolState] Entered Patrol");

        //enemy.anim.SetBool("walk", true);
        enemy.SelectPatrolTarget();
    }

    public override void OnExit()
    {
        //if (enemy != null)
        //    enemy.anim.SetBool("walk", false);

        Debug.Log("[PatrolState] Exit Patrol");
    }

    public override void PhysicsUpdate()
    {
      
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
