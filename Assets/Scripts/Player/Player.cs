using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


public class Player : Character
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;

    [Header("Movement")]
    public float speed;
    public float climbSpeed;
    public float jumpForce;
    public Vector2 debugVelocity;

    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    public bool isDashing = false;     
    public bool canDash = true;        


    [Header("Damage Attributes")]
    public bool isHurt;
    public bool isDead;
    public bool isGroundAttack;

    [Header("Physics Material")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    
    private IInteractable nearbyInteractable;
    public bool canClimb = false;
    public bool _isClimbing = false;
    private Vector3 ladderPosition;
    private float originalGravityScale;

    public bool enableInvincibility = true;
    public bool enableKnockback = true;
    public float knockbackForce = 10f;

    public static event Action<bool> OnClimbStateChanged;
    private PlayerController playerController;
      
    public Material defaultMaterial;
    public Material dashMaterial;
    public ParticleSystem dashEffect;
    public ParticleSystem dashSpeedLine;

    public bool isClimbing
    {
        get => _isClimbing;
        set
        {
            if (_isClimbing != value) // **只有在状态真正改变时才触发事件**
            {
                _isClimbing = value;
                OnClimbStateChanged?.Invoke(_isClimbing); // **触发事件**
            }
        }
    }
    protected virtual void FixedUpdate()
    {
        debugVelocity = rb.linearVelocity;
    }
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        playerController = GetComponent<PlayerController>();
        originalGravityScale = rb.gravityScale;
        GetComponent<SpriteRenderer>().material.SetFloat("_FlashControl", 1);
        DamageEffectHandler effectHandler = GetComponent<DamageEffectHandler>();
        if (effectHandler == null)
        {
            effectHandler = gameObject.AddComponent<DamageEffectHandler>();
        }
        effectHandler.Initialize(GetComponent<SpriteRenderer>());

        damageHandler = new DamageHandler(this, effectHandler);
        // Subscribe take damage event
        OnTakeDamage += (_, _, _, _ )=>playerAnimation.PlayHurt();
        OnTakeDamage += GetHurt;

        defaultMaterial = spriteRenderer.material;



    }
    protected override void Update()
    {
        base.Update();

        if (isClimbing && physicsCheck.isGround)
        {
            ExitClimb();
        }

        CheckState();
    }
    public void Move(Vector2 inputDirection)
    {
        if (isHurt || isGroundAttack || isDashing) return;
        if (isClimbing)
        {
            if (physicsCheck.isGround) 
            {
                ExitClimb();
            }
            //rb.linearVelocity = new Vector2(0, inputDirection.y * climbSpeed);
            return;
        }

        if ((physicsCheck.touchLeftWall && inputDirection.x < 0) || (physicsCheck.touchRightWall && inputDirection.x > 0))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
        }
        else
        {
            
            rb.linearVelocity = new Vector2(inputDirection.x * speed, rb.linearVelocity.y);
        }

        
        if (inputDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (inputDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }



    public void Jump()
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up* jumpForce, ForceMode2D.Impulse);

        }
    }

    public IEnumerator DashCoroutine()
    {
        Debug.Log("DashCoroutine!!!");
        isDashing = true;
        canDash = false;
        TriggerInvincible(dashDuration);
        playerController.DisableInput();
        spriteRenderer.material = dashMaterial;
        
        rb.linearVelocity = Vector2.zero;  
        StartDashEffect();
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed,0);
        

        yield return new WaitForSeconds(dashDuration);
        playerController.EnableInput();
        EndDashEffect();
        isDashing = false;
        rb.linearVelocity = Vector2.zero; 
        spriteRenderer.material = defaultMaterial;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public void Dash()
    {
        if (canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Attack()
    {
        playerAnimation.PlayAttack();
        if (physicsCheck.isGround)
        {
            isGroundAttack = true;
        }
        if (isClimbing) 
        {
            ExitClimb();
        }
    }
    public void Climb(float verticalInput)
    {
        if (canClimb)
        {
            if (Mathf.Abs(verticalInput) > 0)
            {
                isClimbing = true;
                //// **让 Player 的 x 轴对齐梯子**
                //transform.position = new Vector3(ladderPosition.x, transform.position.y, transform.position.z);


                rb.linearVelocity = new Vector2(0, verticalInput * climbSpeed);
                rb.gravityScale = 0;

                playerAnimation.SetClimbing(true, Mathf.Abs(verticalInput) > 0);
            }
            else if (isClimbing)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                playerAnimation.SetClimbing(true, false); 

            }
        }
    }

    public void ExitClimb() 
    {
        if (isClimbing)
        {
            isClimbing = false;
            
            rb.gravityScale = originalGravityScale;
            playerAnimation.SetClimbing(false, false);
        }

    }


    // TODO:save for fix the issue of player can't climb the ladder at the center of the ladder
    //public void SetCanClimb(bool state, Vector3 ladder)
    //{
    //    canClimb = state;
    //    ladderPosition = ladder;
    //    if (canClimb)
    //    {
    //         
    //    }
    //    else
    //    {
    //        isClimbing = false;
    //        rb.gravityScale = originalGravityScale;
            
    //    }
    //}

    public void SetCanClimb(bool state)
    {
        canClimb = state;
        if (!canClimb)
        {
            isClimbing = false;
            rb.gravityScale = originalGravityScale;
            playerAnimation.SetClimbing(false, false); 

        }
    }

    public bool CanClimb()
    {
        return canClimb;
    }
    public void Interact()
    {
        if (nearbyInteractable != null)
        {
            nearbyInteractable.Interact();
        }
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHealthChange?.Invoke(this);
    }

    public void GetHurt(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        Debug.Log("Player Get Hurt");
        isHurt = true;
        //rb.linearVelocity = Vector2.zero;

        //Vector2 forceDirection = ((Vector2)transform.position - (Vector2)attacker.position).normalized;
        //if (forceDirection == Vector2.zero)
        //{
        //    forceDirection = new Vector2(0.1f, 0.1f).normalized;
        //}

        //Vector2 force = forceDirection * 10f;
        //rb.AddForce(force, ForceMode2D.Impulse);
        damageHandler.HandleDamage(attacker, damage, knockbackForce, damageType);

        //StartCoroutine(HurtEffect(spriteRenderer, spriteRenderer.material));
    }


    public override void Die()
    {
        if (isDead) return; 
        isDead = true;

        Debug.Log("Player is dead!");

        
        rb.linearVelocity = Vector2.zero;

        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.DisableInput();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            nearbyInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && nearbyInteractable == interactable)
        {
            nearbyInteractable = null;
        }
    }

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
    public void DestoryCollider()
    {
        coll.enabled = false;
    }
    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }

    public override void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        if (isInvincible) return;

        currentHP = Mathf.Max(currentHP - damage, 0);
        if (currentHP <= 0)
        {
            OnDie?.Invoke(transform);
        }
        else
        {
            //TriggerInvincible();
            InvokeTakeDamageEvent(attacker, damage, knockbackForce, damageType);
        }

        OnHealthChange?.Invoke(this);
    }

    void StartDashEffect()
    {
        // 激活粒子特效
        dashEffect.gameObject.SetActive(true);
        // 控制残影方向为当前人物方向
        dashEffect.GetComponent<ParticleSystemRenderer>().flip = new Vector3(transform.localScale.x < 0 ? 0 : 1, 0, 0);
        //dashEffect.transform.localScale = new Vector3(transform.localScale.x < 0 ? -1 : 1, 1, 1);
        //dashEffect.transform.rotation = Quaternion.Euler(0, transform.localScale.x < 0 ? 180 : 0, 0);
        //dashSpeedLine.gameObject.SetActive(true);
    }

    void EndDashEffect()
    {
        // 停止粒子特效
        dashEffect.gameObject.SetActive(false);
        //dashSpeedLine.gameObject.SetActive(false);

    }
}
