using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Attack,
    SpecialSkill,
    Hurt,
    Die,
    Summon
}

public enum SkillType
{
    ExplosiveNotes,
    LaserNotes,
    SharpShield
}

public class BossFSM : MonoBehaviour
{
    public BossState currentState;

    public float health = 500f;
    public float maxHealth = 500f;

    // 技能冷却
    public static float specialSkillCooldown = 10f;
    [SerializeField] private float invincibleTimer = 0.2f;
    [SerializeField] private float specialSkillTimer = 12f;

    // private bool summonEnabled = true;
    private bool summonEnabled = false;

    private bool skillSelected = false;
    [SerializeField] private SpecialSkills skill;

    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private Vector3[] minionPos;

    [SerializeField] private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        ChangeState(BossState.Idle);
    }

    private void Update()
    {
        if (specialSkillTimer >= 0)
            specialSkillTimer -= Time.deltaTime;

        if (invincibleTimer >= 0)
            invincibleTimer -= Time.deltaTime;

        switch (currentState)
        {
            case BossState.Idle:
                UpdateIdleState();
                break;
            // case BossState.Summon:
            //     UpdateSummonState();
            //     break;
            case BossState.SpecialSkill:
                UpdateSpecialSkillState();
                break;
            case BossState.Hurt:
                // Hurt动画
                break;
            case BossState.Die:
                break;

        }
    }

    private void UpdateIdleState()
    {
        // BossMovement控制idle行为
        if (specialSkillTimer <= 0)
        {
            specialSkillTimer = specialSkillCooldown;
            if (summonEnabled)
            {
                ChangeState(BossState.Summon);
                summonEnabled = false;
            } else {
                ChangeState(BossState.SpecialSkill);
            }
        }

    }

    private void UpdateSpecialSkillState()
    {
        if (!skillSelected)
        {
            int randomSkill = Random.Range(0, 2);
            // int randomSkill = 1;
            animator.SetTrigger("SpecialSkill");

            switch (randomSkill)
            {
                case 0:
                    skill.ExplosiveNotes();
                    break;
                case 1:
                    skill.LaserNotes();
                    break;
                // case 2:
                //     skill.SharpShield();
                //     break;
            }

            skillSelected = true;
        }
    }

    // public void UpdateSummonState()
    // {
    //     if (summonEnabled)
    //     {
    //         animator.SetTrigger("Summon");
    //         summonEnabled = false;
    //     }
    // }

    public void SpecialSkillAnimationEnd()
    {
        skillSelected = false;
        ChangeState(BossState.Idle);
    }

    public void SummonAnimationEnd()
    {
        SummonMinions();
        ChangeState(BossState.Idle);
    }

    private void SummonMinions()
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPosition = minionPos[i];

            Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void ChangeState(BossState newState)
    {
        // ExitState(currentState);

        currentState = newState;

        EnterState(newState);
        Debug.Log("Boss state: " + newState);
    }

    // 进入状态
    private void EnterState(BossState state)
    {
        switch (state)
        {
            case BossState.Idle:
                animator.SetTrigger("Idle");
                break;
            case BossState.SpecialSkill:
                specialSkillCooldown = 20f;
                animator.SetTrigger("SpecialSkill");
                break;
            case BossState.Hurt:
                animator.SetTrigger("Hurt");
                break;
            case BossState.Die:
                animator.SetTrigger("Die");
                GetComponent<Collider2D>().enabled = false;
                break;
            case BossState.Summon:
                animator.SetTrigger("Summon");
                break;
        }
    }

    public void TakeDamage(float damage, DamageType damageType)
    {

        if(invincibleTimer > 0)
        {
            return;
        }
        
        health -= damage;

        invincibleTimer = 0.2f;

        if (health <= 0)
        {
            health = 0;
            ChangeState(BossState.Die);
        }
        else if (currentState == BossState.Idle)
        {
            ChangeState(BossState.Hurt);
        }
    }

    // 受伤动画结束回调
    public void HurtAnimationEnd()
    {
        ChangeState(BossState.Idle);
    }

    public void DieAnimationEnd()
    {
        Destroy(gameObject);
    }
}