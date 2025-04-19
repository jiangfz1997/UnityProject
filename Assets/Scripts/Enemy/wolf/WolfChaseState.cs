using UnityEngine;

public class WolfChaseState : GenericChaseState
{


    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        Debug.Log("[Wolf] Enter Chase State");
    }
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
