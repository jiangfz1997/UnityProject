using UnityEngine;
using System.Collections;
public class BoarPatrolState : BaseState
{
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            Debug.Log("Player found, switching to Chase state.："+EnemyState.Chase);

            currentEnemy.SwitchState(EnemyState.Chase);
        }

        if (currentEnemy.physicsCheck.isCliffAhead || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.isWaiting = true;
            currentEnemy.currentSpeed = 0;
            currentEnemy.rb.linearVelocity = Vector2.zero; // 设置 Rigidbody2D 的速度为零

            currentEnemy.anim.SetBool("walk", false);
        }
        else 
        { 
            currentEnemy.anim.SetBool("walk", true);
        }
        
    }

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("walk", true);
        currentEnemy.physicsCheck.Check();
        Debug.Log("Enter Patrol State walk");

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
        Debug.Log("Exit Patrol State");
    }

    public override void PhysicsUpdate()
    {
        //Debug.Log("Physics Update");
    }
}
