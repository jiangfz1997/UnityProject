using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhantomState
{
    Idle,
    Attack,
    Walk,
    SpecialSkill,
    Enrage,
    Hurt,
    Die
}

public class PhantomFSM : MonoBehaviour
{
    public PhantomState currentState;

    public float health = 10f;
    public float maxHealth = 10f;

    public static float specialSkillCooldown = 15f;
    public static float attackCooldown = 8f;
    public static float enrageCoolDown = 5f;
    public float moveSpeed = 2f;
    public float attackRange = 4f;
    [SerializeField] private float invincibleTimer = 0.2f;
    [SerializeField] private float specialSkillTimer = 15f;
    [SerializeField] private float attackTimer = 0f;

    private bool skillSelected = false;
    [SerializeField] private PhantomMirrors specialSkill;
    [SerializeField] private GameObject SmogPrefab;
    [SerializeField] private Vector3 offset = new Vector3(3f, 1f, 0f);

    private Animator animator;
    private Transform transform;
    private Rigidbody2D rb;
    private Transform playerTransform;

    public event Action OnPhantomDeath;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeState(PhantomState.Idle);
    }

    private void Update()
    {
        if (currentState == PhantomState.Die)
        {
            UpdateDieState();
            return;
        }

        if (attackTimer >= 0)
            attackTimer -= Time.deltaTime;

        if (specialSkillTimer >= 0)
            specialSkillTimer -= Time.deltaTime;

        if (invincibleTimer >= 0)
            invincibleTimer -= Time.deltaTime;

        switch (currentState)
        {
            case PhantomState.Idle:
                UpdateIdleState();
                break;
            case PhantomState.Walk:
                ChaseAndAttack();
                break;
            case PhantomState.Enrage:
                UpdateEnrageState();
                break;
            case PhantomState.SpecialSkill:
                UpdateSpecialSkillState();
                break;
            case PhantomState.Hurt:
                // Hurt Animation
                break;
        }
    }

    private void UpdateIdleState()
    {
        FacePlayer();

        if (health <= 0)
        {
            ChangeState(PhantomState.Die);
            return;
        }

        if (health <= 0.2 * maxHealth)
        {
            ChangeState(PhantomState.Enrage);
        }

        if (attackTimer <= 0)
        {
            ChangeState(PhantomState.Walk);
        }
        else if (specialSkillTimer <= 0)
        {
            // ChangeState(PhantomState.SpecialSkill);
        }

    }

    private void ChaseAndAttack()
    {
        Vector2 directionToPlayer = GetDirectionToPlayer();

        FacePlayer();

        if (GetDistanceToPlayer() <= attackRange)
        {
            if (attackTimer <= 0)
            {
                attackTimer = attackCooldown;
                Invoke("Attack", 0.3f);
            }
        }
        else
        {
            rb.linearVelocity = directionToPlayer * moveSpeed;
        }
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Invoke SmogAttack");
        attackTimer = attackCooldown;
        ChangeState(PhantomState.Attack);
    }

    Vector2 GetDirectionToPlayer()
    {
        if (playerTransform != null)
        {
            return (playerTransform.position.x > transform.position.x) ? Vector2.right : Vector2.left;
        }
        return Vector2.zero;
    }

    float GetDistanceToPlayer()
    {
        if (playerTransform != null)
        {
            return Vector2.Distance(transform.position, playerTransform.position);
        }
        return Mathf.Infinity;
    }

    public void StartSmogAnimation()
    {
        if (transform.localScale.x < 0)
            offset.x = -Mathf.Abs(offset.x);
        else
            offset.x = Mathf.Abs(offset.x);

        Instantiate(SmogPrefab, transform.position + offset, Quaternion.identity);
    }

    public void EndAttackAnimation()
    {
        ChangeState(PhantomState.Idle);
    }

    private void UpdateEnrageState()
    {
        attackCooldown = enrageCoolDown;

        if (attackTimer <= 0)
        {
            ChangeState(PhantomState.Walk);
        }

    }

    private void UpdateSpecialSkillState()
    {
        specialSkillTimer = specialSkillCooldown;
        specialSkill.Cast();

    }

    private void UpdateDieState()
    {
        OnPhantomDeath?.Invoke();
        FacePlayer();
    }

    private void FacePlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        Vector3 newScale = transform.localScale;
        newScale.x = shouldFaceRight ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }


    public void SpecialSkillAnimationEnd()
    {
        skillSelected = false;
        ChangeState(PhantomState.Idle);
    }

    public void ChangeState(PhantomState newState)
    {

        currentState = newState;

        EnterState(newState);
        // Debug.Log("Phantom state: " + newState);
    }


    private void EnterState(PhantomState state)
    {
        switch (state)
        {
            case PhantomState.Idle:
                animator.SetTrigger("Idle");
                break;
            case PhantomState.Walk:
                animator.SetTrigger("Walk");
                break;
            case PhantomState.Attack:
                animator.SetTrigger("Attack");
                break;
            case PhantomState.SpecialSkill:
                animator.SetTrigger("SpecialSkill");
                break;
            case PhantomState.Hurt:
                animator.SetTrigger("Hurt");
                break;
            case PhantomState.Die:
                animator.SetTrigger("Dying");
                break;
        }
    }

    public void TakeDamage(float damage, DamageType damageType)
    {
        if (invincibleTimer > 0 || damage == 0)
        {
            return;
        }

        Debug.Log("Boss Take Damage: " + damage + " Type: " + damageType);

        health -= damage;

        invincibleTimer = 0.2f;

        if (health <= 0)
        {
            health = 0;
            ChangeState(PhantomState.Die);
        }
        else if (currentState == PhantomState.Idle)
        {
            ChangeState(PhantomState.Hurt);
        }
    }

    public void HurtAnimationEnd()
    {
        ChangeState(PhantomState.Idle);
    }


}