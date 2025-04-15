using UnityEngine;

public class BlackEliteMeleeState : BaseState
{
    private BlackElite enemy;
    private bool isAttacking = false;

    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as BlackElite;
        if (enemy == null)
        {
            Debug.LogError("[MeleeState] Enemy is not BlackElite!");
            return;
        }

        Debug.Log("[BlackEliteMeleeState] Entered Melee Attack");

        isAttacking = true;

        enemy.FreezeMovement();

        enemy.anim.SetTrigger("melee");
        enemy.meleeTimer = enemy.meleeCooldown;

    }

    public override void LogicUpdate()
    {
        if (isAttacking) return;
    }

    public override void OnExit()
    {
        Debug.Log("[BlackEliteMeleeState] Exit Melee Attack");
        isAttacking = false;
    }
 
    public override void PhysicsUpdate() { }

    
    public void OnMeleeEnd()
    {
        Debug.Log("[BlackEliteMeleeState] Melee attack end");

        isAttacking = false;

        enemy.SwitchState(enemy.chaseState);
    }
}
