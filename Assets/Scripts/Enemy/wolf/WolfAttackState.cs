using UnityEngine;

public class WolfAttackState : GenericAttackState
{
    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        enemy.rb.linearVelocity = Vector2.zero;
        //enemy.anim.SetTrigger("Attack");
        Debug.Log("[Slime] Enter Attack State");
    }

    protected override void DoAttack()
    {
        enemy.anim.SetTrigger("Attack");
        Debug.Log("[Wolf] Attack");
    }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        base.OnExit();
        //enemy.anim.SetBool("Attack", false);
    }
}
