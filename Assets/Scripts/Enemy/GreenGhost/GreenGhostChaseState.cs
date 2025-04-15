using UnityEngine;

public class GreenGhostChaseState : GenericChaseState
{
    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        Debug.Log("[GreenGhost] Enter Chase State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        base.OnExit();
        //enemy.anim.SetBool("Chase", false);
        Debug.Log("[GreenGhost] Exit Chase State");
    }
}
