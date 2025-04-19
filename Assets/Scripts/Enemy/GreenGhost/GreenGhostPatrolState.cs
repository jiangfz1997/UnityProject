using UnityEngine;

public class GreenGhostPatrolState : GenericPatrolState
{
    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        //enemy.anim.SetBool("walk", true);
        Debug.Log("[GreenGhost] Enter Patrol State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.IsPlayerInSight())
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }
    }

    public override void PhysicsUpdate() { }
    public override void OnExit()
    {
        //enemy.anim.SetBool("walk", false);
        Debug.Log("[GreenGhost] Exit Patrol State");
    }

}
