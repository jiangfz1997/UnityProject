using System.Collections;
using UnityEngine;

public class PurpleEliteSpellState : BaseState
{
    private PurpleElite enemy;
    private bool isCasting = false;
    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as PurpleElite;
        if (enemy == null)
        {
            Debug.LogError("[SpellState] Enemy is not PurpleElite!");
            return;
        }

        Debug.Log("[SpellState] Entered SpellCast");

        enemy.FreezeMovement(); 
        enemy.StartCoroutine(CastSpell());
    }
    private IEnumerator CastSpell()
    {
        isCasting = true;

     
        enemy.anim.SetTrigger("cast");

        // �ȴ�ʩ��ǰҡʱ�䣨���ö����¼������
        yield return new WaitForSeconds(0.5f);

        // ��������
        //SpawnLightningAtPlayer();

        // ���÷�����ȴ
        enemy.spellTimer = enemy.spellCooldown;

        // ��ҡ���ɼ���Ч����Ч�ȣ�
        yield return new WaitForSeconds(0.4f);

        // �ص�׷��״̬
        enemy.SwitchState(enemy.chaseState);

        isCasting = false;
    }

    
    public override void LogicUpdate()
    {
        if (isCasting) return;
    }

    

    public override void OnExit()
    {
        Debug.Log("[SpellState] Exit SpellCast");
    }

    public override void PhysicsUpdate() { }

  
}
