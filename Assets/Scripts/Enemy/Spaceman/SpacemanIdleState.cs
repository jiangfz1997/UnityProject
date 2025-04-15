using UnityEngine;

public class SpacemanIdleState : GenericPatrolState
{
    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        //enemy.anim.SetBool("walk", false);
        Debug.Log("[Slim] Enter Idle State");
    }
    public override void LogicUpdate()
    {
        if (enemy.IsPlayerInSight())
        {
            enemy.SwitchState(EnemyState.Chase);
            //Debug.Log("[Slim] Player detected, switching to Chase state.");
            return;
        }

        // 如果超出巡逻限制，重新选择目标
        if (!enemy.InsideOfLimits())
        {
            enemy.SelectPatrolTarget();
            //Debug.Log("[Slim] Out of patrol limits, selecting new patrol target.");
            return;
        }
        if (enemy.physicsCheck.isCliffAhead || (enemy.physicsCheck.touchLeftWall && enemy.faceDir.x < 0) || (enemy.physicsCheck.touchRightWall && enemy.faceDir.x > 0))
        {
            enemy.SelectPatrolTarget();
            //Debug.Log("[Slim] Cliff or wall detected, selecting new patrol target.");
            return;
        }
        // 向当前目标移动
        enemy.MoveTo(enemy.patrolTarget.position);

        // 如果靠近目标，切换巡逻方向

        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.patrolTarget.position);
        //Debug.Log($"[Slim][STATE LOGIC] Patrol | {enemy.name} | distanceToTarget = {distanceToTarget}");
        if (distanceToTarget < 0.2f)
        {
            enemy.SelectPatrolTarget();
        }
    }
    public override void PhysicsUpdate() { }
    public override void OnExit()
    {
        //enemy.anim.SetBool("walk", false);
        Debug.Log("[Slime] Exit Idle State");

    }
}

