using UnityEngine;

public class BlackEliteShootState : BaseState
{
    private BlackElite enemy;
    private bool isShooting = false;
    public override void LogicUpdate()
    {
        if (isShooting) return;
    }
    public override void OnEnter(Enemy enemy)
    {
        this.enemy = enemy as BlackElite;
        if (this.enemy == null)
        {
            Debug.LogError("[ShootState] Enemy is not BlackElite!");
            return;
        }

        isShooting = true;
        this.enemy.FreezeMovement();
        this.enemy.anim.SetTrigger("shoot");
        this.enemy.shootTimer = this.enemy.shootCooldown;

    }
    public override void OnExit()
    {
        Debug.Log("[ShootState] Exit Shoot");
        isShooting = false;
    }
    public override void PhysicsUpdate() { }
    public void OnShootEnd() 
    {
        isShooting = false;
        enemy.SwitchState(enemy.chaseState);
    }
}
   

