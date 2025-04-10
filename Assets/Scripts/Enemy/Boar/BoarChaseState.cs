using UnityEngine;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class BoarChaseState : BaseState
{
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(EnemyState.Patrol);
        }
        if (currentEnemy.physicsCheck.isCliffAhead || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(-currentEnemy.transform.localScale.x, 1, 1);
        }
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        Debug.Log("Enter Chase State");
        currentEnemy.isChasing = true;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("chase", true);
    }



    public override void PhysicsUpdate()
    {
        
      
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", false);
        Debug.Log("Exit Chase State");
    }
}
