using System.Collections;
using UnityEngine;

public class GenericAttackState : BaseState
{
    protected Enemy enemy;
    protected bool isAttacking = false; // ✅ 是否正在攻击

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnEnter(Enemy enemy)
    {
        this.enemy = enemy;
        //this.enemy.anim.SetBool("walk", false);

        Debug.Log($"[STATE ENTER] Attack | {enemy.name} | currentSpeed = {enemy.currentSpeed}");

        //Transform player = enemy.hit.collider?.transform;
        //if (player != null)
        //{
        //    Vector2 dir = player.position - enemy.transform.position;
        //    if (dir.x != 0)
        //        enemy.transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
        //}
    }
    //public override void LogicUpdate()
    //{
    //    if (!enemy.IsPlayerInSight())
    //    {
    //        enemy.SwitchState(EnemyState.Chase);
    //        return;
    //    }

    //    Transform player = enemy.hit.collider?.transform;
    //    if (player == null)
    //    {
    //        enemy.SwitchState(EnemyState.Chase);
    //        return;
    //    }

    //    float distance = Vector2.Distance(enemy.transform.position, player.position);

    //    if (enemy.isAttackCoolingDown || distance > enemy.attackDistance)
    //    {
    //        enemy.SwitchState(EnemyState.Chase);
    //        return;
    //    }

    //    // 攻击冷却计时


    //    DoAttack();
    //    enemy.SetAttackCoolDown();


    //}

    public override void LogicUpdate()
    {
        if (isAttacking) return;

        if (!enemy.IsPlayerInSight())
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        Transform player = enemy.hit.collider?.transform;
        if (player == null)
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, player.position);

        if (enemy.isAttackCoolingDown || distance > enemy.attackDistance)
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        enemy.StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        float originalSpeed = enemy.currentSpeed;
        enemy.FreezeMovement();
        DoAttack();

        yield return new WaitForSeconds(0.4f); // 动画本体时间

        enemy.SetAttackCoolDown();

        yield return new WaitForSeconds(1f);

        enemy.currentSpeed = originalSpeed;
        isAttacking = false;

        enemy.SwitchState(EnemyState.Chase);
    }
    protected virtual void DoAttack() { }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
    }


}
