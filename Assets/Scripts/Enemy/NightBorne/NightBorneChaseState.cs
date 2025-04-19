using UnityEngine;

public class NightBorneChaseState : GenericChaseState
{
    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        Debug.Log("[Slime] Enter Chase State");
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
        Debug.Log("[Slime] Exit Chase State");

    }
}
