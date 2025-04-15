using UnityEngine;

public class BlackEliteChaseState : BaseState
{
    private BlackElite enemy;
    private float dashChance = 0.4f; 


    public override void LogicUpdate()
    {
        if (enemy == null || Player.Instance == null) return;

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


        bool inMeleeRange = enemy.IsInMeleeRange();
        bool inDashRange = enemy.IsInDashRange();
        bool inShootRange = enemy.IsInShootRange();

        bool meleeReady = enemy.IsMeleeReady();
        bool dashReady = enemy.IsDashReady();
        bool shootReady = enemy.IsShootReady();




        if (inMeleeRange && meleeReady)
        {
            if (dashReady && Random.value < dashChance)
            {
                Debug.Log("[ChaseState] Dash in melee range");
                enemy.SwitchState(enemy.dashAttackState);
            }
            else
            {
                Debug.Log("[ChaseState] Melee attack");
                enemy.SwitchState(enemy.meleeState);
            }
            return;
        }

        if (inDashRange && dashReady)
        {
            if (Random.value < dashChance)
            {
                Debug.Log("[ChaseState] Dash from mid-range");
                enemy.SwitchState(enemy.dashAttackState);
                return;
            }
        }

        if (inShootRange && shootReady)
        {
            Debug.Log("[ChaseState] Ranged attack");
            enemy.SwitchState(enemy.shootState);
            return;
        }

        // Step 6: ¼ÌÐø×·×Ù
        enemy.MoveTo(Player.Instance.transform.position);
    }

    public override void OnEnter(Enemy baseEnemy)
    {
        enemy = baseEnemy as BlackElite;
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

    public override void PhysicsUpdate() { }

    
}
