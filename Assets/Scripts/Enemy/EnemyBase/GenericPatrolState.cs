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

        // �������Ѳ�����ƣ�����ѡ��Ŀ��
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
        // ��ǰĿ���ƶ�
        enemy.MoveTo(enemy.patrolTarget.position);

        // �������Ŀ�꣬�л�Ѳ�߷���
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
