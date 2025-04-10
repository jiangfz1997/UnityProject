using UnityEngine;

public class GenericAttackState : BaseState
{
    protected Enemy enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnEnter(Enemy enemy)
    {
        this.enemy = enemy;
        //this.enemy.anim.SetBool("walk", false);

        Debug.Log($"[STATE ENTER] Attack | {enemy.name} | currentSpeed = {enemy.currentSpeed}");

        //Transform player = enemy.hit.collider?.transform;
        //if (player != null)
        //{
        //    Vector2 dir = player.position - enemy.transform.position;
        //    if (dir.x != 0)
        //        enemy.transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
        //}
    }
    public override void LogicUpdate()
    {
        if (!enemy.IsPlayerInSight())
        {
            enemy.SwitchState(EnemyState.Patrol);
            return;
        }

        Transform player = enemy.hit.collider?.transform;
        if (player == null)
        {
            enemy.SwitchState(EnemyState.Patrol);
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, player.position);

        if (enemy.isAttackCoolingDown || distance > enemy.attackDistance)
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        // ¹¥»÷ÀäÈ´¼ÆÊ±

       
        DoAttack();
        enemy.SetAttackCoolDown();
        

    }
    protected virtual void DoAttack() { }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
    }
}
