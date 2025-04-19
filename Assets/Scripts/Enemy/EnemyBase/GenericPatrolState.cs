using UnityEngine;

public class GenericPatrolState : BaseState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Enemy enemy;

    public override void OnEnter(Enemy enemy)
    {
        this.enemy = enemy;
        this.enemy.SelectPatrolTarget();
        this.enemy.isChasing = false;
        this.enemy.currentSpeed = this.enemy.normalSpeed;
        Debug.Log($"[STATE ENTER] Patrol | {enemy.name} | currentSpeed = {enemy.currentSpeed}");

    }

    public override void LogicUpdate()
    {
        if (enemy.IsPlayerInSight())
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        // 如果超出巡逻限制，重新选择目标
        if (!enemy.InsideOfLimits())
        {
            enemy.SelectPatrolTarget();
            return;
        }
        if (enemy.physicsCheck.isCliffAhead || (enemy.physicsCheck.touchLeftWall && enemy.faceDir.x < 0) || (enemy.physicsCheck.touchRightWall && enemy.faceDir.x > 0))
        {
            enemy.SelectPatrolTarget();
            return;
        }
        // 向当前目标移动
        enemy.MoveTo(enemy.patrolTarget.position);

        // 如果靠近目标，切换巡逻方向
        //float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.patrolTarget.position);
        float distanceX = Mathf.Abs(enemy.transform.position.x - enemy.patrolTarget.position.x);

        if (distanceX < 0.2f)
        {
            enemy.SelectPatrolTarget();
        }
    }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        //enemy.anim.SetBool("walk", false);
    }
}
