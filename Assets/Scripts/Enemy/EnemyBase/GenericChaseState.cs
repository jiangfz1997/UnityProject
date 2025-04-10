using UnityEngine;

public class GenericChaseState : BaseState
{
    public Enemy enemy;
    public override void OnEnter(Enemy enemy)
    {
        this.enemy = enemy;
        //this.enemy.anim.SetBool("walk", true);
        this.enemy.isChasing = true;
        //this.enemy.currentSpeed = this.enemy.chaseSpeed;
        Debug.Log($"[STATE ENTER] chase | {enemy.name} | currentSpeed = {enemy.currentSpeed}");

    }

    public override void LogicUpdate()
    {

        if (!enemy.IsPlayerInSight())
        {
            // ��ʧĿ���ʱ
            enemy.lostTargetTimer += Time.deltaTime;
            if (enemy.lostTargetTimer >= enemy.patrolCooldown)
            {
                Debug.Log("[Trashbag] ��Ҷ�ʧ���ص�Ѳ��");
                enemy.lostTargetTimer = 0f;
                enemy.SwitchState(EnemyState.Patrol);
            }
            if (enemy.physicsCheck.isCliffAhead || (enemy.physicsCheck.touchLeftWall && enemy.faceDir.x < 0) || (enemy.physicsCheck.touchRightWall && enemy.faceDir.x > 0))
            {
                enemy.transform.localScale = new Vector3(enemy.faceDir.x, 1, 1);
            }
            return;
        }
        else
        {
            enemy.lostTargetTimer = 0f; // ���ü�ʱ��
        }

        // ��ȡ���λ�ò�׷��
        Transform player = enemy.hit.collider?.transform;
        if (player == null)
        {
            enemy.SwitchState(EnemyState.Patrol);
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, player.position);

        if (distance > enemy.attackDistance || enemy.isAttackCoolingDown)
        {
            if (enemy.physicsCheck.isCliffAhead)
            {
                return;
                enemy.transform.localScale = new Vector3(enemy.faceDir.x, 1, 1);
            }
            enemy.MoveTo(player.position);
        }
        else
        {
            enemy.SwitchState(EnemyState.Attack);
        }
    }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        //enemy.anim.SetBool("walk", false);
    }
}
