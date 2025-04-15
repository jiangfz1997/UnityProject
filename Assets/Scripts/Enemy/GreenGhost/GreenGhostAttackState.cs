using System;
using UnityEngine;

public class GreenGhostAttackState : GenericAttackState
{
    new GreenGhost enemy;

    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        this.enemy = enemy as GreenGhost;
        enemy.rb.linearVelocity = Vector2.zero;
        Debug.Log("[GreenGhost] Enter Attack State");
    }

    protected override void DoAttack()
    {
        enemy.anim.SetTrigger("Attack");

        Debug.Log("[GreenGhost] Attack");
    }

    

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("[GreenGhost] Exit Attack State");
    }
}
