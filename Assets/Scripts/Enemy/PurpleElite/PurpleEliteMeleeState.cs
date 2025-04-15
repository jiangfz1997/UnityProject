using System.Collections;
using UnityEngine;

public class PurpleEliteMeleeState : BaseState
{
    private PurpleElite enemy;
    private bool isAttacking = false;
    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as PurpleElite;
        if (enemy == null)
        {
            Debug.LogError("[MeleeState] Enemy is not PurpleElite!");
            return;
        }

        Debug.Log("[MeleeState] Entered Melee Attack");

        // Freeze movement
        enemy.FreezeMovement();

        // Start attack logic
        enemy.StartCoroutine(PerformMeleeAttack());
    }
    private IEnumerator PerformMeleeAttack()
    {
        isAttacking = true;

        // Play attack animation
        enemy.anim.SetTrigger("melee");

        // Wait for the attack animation to hit (�ɸ��ݶ���ʱ������)
        yield return new WaitForSeconds(0.4f); // ǰҡ

        

        // ���ý�ս��ȴ
        enemy.meleeTimer = enemy.meleeCooldown;

        // ��ҡʱ��
        yield return new WaitForSeconds(0.7f);

        // �ص�׷��״̬
        enemy.SwitchState(enemy.chaseState);
        isAttacking = false;
    }
   
    public override void LogicUpdate()
    {
        // prevent switching while attacking
        if (isAttacking) return;
    }

    public override void OnExit()
    {
        Debug.Log("[MeleeState] Exit Melee Attack");
    }

    public override void PhysicsUpdate() { }

}
