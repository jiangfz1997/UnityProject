using UnityEngine;

public class TrashbagChaseState : GenericChaseState
{


    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        Debug.Log("[Trashbag] Enter Chase State");
    }


    //public override void LogicUpdate()
    //{
    //    if (!enemy.IsPlayerInSight())
    //    {
    //        // 丢失目标计时
    //        enemy.lostTargetTimer += Time.deltaTime;
    //        if (enemy.lostTargetTimer >= enemy.patrolCooldown)
    //        {
    //            Debug.Log("[Trashbag] 玩家丢失，回到巡逻");
    //            enemy.lostTargetTimer = 0f;
    //            enemy.SwitchState(EnemyState.Patrol);
    //        }
    //        return;
    //    }
    //    else
    //    {
    //        enemy.lostTargetTimer = 0f; // 重置计时器
    //    }

    //    // 获取玩家位置并追踪
    //    Transform player = enemy.hit.collider?.transform;
    //    if (player == null)
    //    {
    //        enemy.SwitchState(EnemyState.Patrol);
    //        return;
    //    }

    //    float distance = Vector2.Distance(enemy.transform.position, player.position);

    //    if (distance > enemy.attackDistance)
    //    {
    //        enemy.MoveTo(player.position);
    //    }
    //    else
    //    {
    //        enemy.SwitchState(EnemyState.Attack);
    //    }
    //}
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        //enemy.anim.SetBool("walk", false);
    }
}
