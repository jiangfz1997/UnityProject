using UnityEngine;

public class PurpleEliteChaseState : BaseState
{
    private PurpleElite enemy;
    public override void LogicUpdate()
    {
        if (enemy == null || Player.Instance == null) return;

        // Check if player is out of vision range or too far
        float distance = Vector2.Distance(enemy.transform.position, Player.Instance.transform.position);
        if (!enemy.isTrackingPlayer && enemy.CanSeePlayer())
        {
            enemy.isTrackingPlayer = true;
        }

        if (enemy.isTrackingPlayer && !enemy.IsPlayerInChaseZone())
        {
            Debug.Log("[Chase] Player left chase zone, stop tracking");
            enemy.isTrackingPlayer = false;
            enemy.SwitchState(enemy.patrolState);
            return;
        }

        // Prioritize melee attack
        if (enemy.IsInMeleeRange() && enemy.IsMeleeReady())
        {
            Debug.Log("[ChaseState] Switching to MeleeAttack");
            enemy.SwitchState(enemy.meleeState);
            return;
        }

        // Fallback to spell cast if in range and spell is ready
        if (enemy.IsInSpellRange() && enemy.IsSpellReady())
        {
            Debug.Log("[ChaseState] Switching to SpellCast");
            enemy.SwitchState(enemy.spellState);
            return;
        }

        // Continue chasing
        enemy.MoveTo(Player.Instance.transform.position);
    }

    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as PurpleElite;
        if (enemy == null)
        {
            Debug.LogError("[ChaseState] Enemy is not PurpleElite!");
            return;
        }
        enemy.currentSpeed = enemy.normalSpeed * enemy.chaseSpeedMultiplier;
        if (enemy.anim != null)
            enemy.anim.speed = enemy.chaseAnimSpeedMultiplier;

        Debug.Log("[ChaseState] Entered Chase");
    }

    public override void OnExit()
    {
        Debug.Log("[ChaseState] Exit Chase");
        if (enemy.anim != null)
            enemy.anim.speed = 1f;
        enemy.currentSpeed = enemy.normalSpeed;
    }

    public override void PhysicsUpdate(){ }
}
