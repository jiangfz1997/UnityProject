using UnityEngine;

public class WolfPatrolState : GenericPatrolState
{
    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);

        enemy.faceDir = new Vector2(1, 0);  // 朝右
        enemy.transform.localScale = new Vector3(1, 1, 1); // 强制朝右外观
        enemy.SelectPatrolTarget(); // 确保目标点在右边

        Debug.Log("[Wolf] Enter Patrol State");
    }
    public override void LogicUpdate()
    {
        if (enemy.IsPlayerInSight())
        {
            enemy.SwitchState(EnemyState.Chase);
            Debug.Log("[Wolf] Player detected, switching to Chase state.");
            return;
        }

        if (!enemy.InsideOfLimits())
        {
            enemy.SelectPatrolTarget();
            Debug.Log("[Wolf] Out of patrol limits, selecting new patrol target.");
            return;
        }
        if (enemy.physicsCheck.isCliffAhead || (enemy.physicsCheck.touchLeftWall && enemy.faceDir.x < 0) || (enemy.physicsCheck.touchRightWall && enemy.faceDir.x > 0))
        {
            enemy.SelectPatrolTarget();
            Debug.Log("[Wolf] Cliff or wall detected, selecting new patrol target.");
            return;
        }
        enemy.MoveTo(enemy.patrolTarget.position);


        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.patrolTarget.position);
        Debug.Log($"[Wolf][STATE LOGIC] Patrol | {enemy.name} | distanceToTarget = {distanceToTarget}");
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

