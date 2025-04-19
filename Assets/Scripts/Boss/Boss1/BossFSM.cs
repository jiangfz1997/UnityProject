using System;
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

public class BossFSM : MonoBehaviour
{
    public BossState currentState;

    public float health = 500f;
    public float maxHealth = 500f;

    public static float specialSkillCooldown = 10f;
    [SerializeField] private float invincibleTimer = 0.2f;
    [SerializeField] private float specialSkillTimer = 12f;

    private bool summonEnabled = true;

    private bool skillSelected = false;
    [SerializeField] private SpecialSkills skill;

    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private Vector3[] minionPos;

    public event Action OnBossDeath;

    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();

        ChangeState(BossState.Idle);
    }

    private void Update()
    {
        if (currentState == BossState.Die)
        {
            OnBossDeath?.Invoke();
            return;
        }

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
                break;
        }
    }

    private void UpdateIdleState()
    {
        // BossMovement����idle��Ϊ
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
            int randomSkill = UnityEngine.Random.Range(0, 2);
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
        for (int i = 0; i < minionPos.Length; i++)
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
        if(invincibleTimer > 0 || damage == 0)
        {
            return;
        }

        Debug.Log("Boss Take Damage: " + damage + " Type: " + damageType);
        
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

    public void HurtAnimationEnd()
    {
        ChangeState(BossState.Idle);
    }

    public void DieAnimationEnd()
    {
        Destroy(gameObject);
    }
}