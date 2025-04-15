using UnityEngine;

public class SlimAttackState : GenericAttackState
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
        Debug.Log("[Slime] Attack");
    }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        base.OnExit();
        //enemy.anim.SetBool("Attack", false);
    }
}
