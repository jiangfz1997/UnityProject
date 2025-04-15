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

        // 等待施法前摇时间（可用动画事件替代）
        yield return new WaitForSeconds(0.5f);

        // 生成闪电
        //SpawnLightningAtPlayer();

        // 设置法术冷却
        enemy.spellTimer = enemy.spellCooldown;

        // 后摇（可加音效、特效等）
        yield return new WaitForSeconds(0.4f);

        // 回到追击状态
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
