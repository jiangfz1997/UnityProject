using UnityEngine;

public class BlackEliteDashAttackState : BaseState
{
    private BlackElite enemy;
    private bool isAnimationFinished;


    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as BlackElite;

        isAnimationFinished = false;
        if (enemy == null)
        {
            Debug.LogError("[DashAttackState] Enemy is not BlackElite!");
            return;
        }
        enemy.anim.SetTrigger("dash");
    }
    public override void LogicUpdate()
    {
        if (isAnimationFinished)
        {
            enemy.SwitchState(enemy.chaseState);
        }
    }
    public override void OnExit()
    {
        Debug.Log("[DashAttackState] Exit Dash Attack");


    }

    public override void PhysicsUpdate()
    {
      
    }
    public void OnDashEnd()
    {
        Debug.Log("[DashAttackState] Dash attack end");
        isAnimationFinished = true;
        enemy.SwitchState(enemy.chaseState);
    }

}
