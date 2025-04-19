using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit.Framework.Interfaces;
using UnityEngine.EventSystems;
using Unity.Cinemachine;


public class Player : Character
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;
    public static Player Instance;

    // Buff manager
    //public BuffSystem buffSystem;
    private ElementSystem elementSystem;

    [Header("Debug Info")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private int gold; 


    [Header("Movement")]
    public float speed;
    public float speedMultiplier = 1f; // Speed multiplier for buffs
    public float climbSpeed;
    public float jumpForce;
    public bool canDoubleJump = true;
    public bool hasJumpedOnce = false;
    public bool hasDoubleJumped = false;
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

    [Header("RangedAttack")]
    public ProjectileDatabase projectileDatabase;
    public Transform firePoint;

    private IInteractable nearbyInteractable;
    public bool canClimb = false;
    public bool _isClimbing = false;
    private Vector3 ladderPosition;
    private float originalGravityScale;
    private PlayerEquipment equipment;
    public bool enableInvincibility = true;
    public bool enableKnockback = true;
    public float knockbackForce = 10f;

    public static event Action<bool> OnClimbStateChanged;
    private PlayerController playerController;
      
    public Material defaultMaterial;
    public Material dashMaterial;
    public GameObject dashEffectRight;
    public GameObject dashEffectLeft;
    private ParticleSystem currentDashEffect;

    public ParticleSystem dashSpeedLine;

    private float DifficultyMultiplier = 1.0f;

    private PlayerStats stats;
    //private bool isAddingItem = false;
    public bool isAttacking = false;
    public Action<Enemy> OnHitEnemy;
    public PlayerSFX playerSFX;


    //public List<ItemData> inventory = new List<ItemData>(); // For extendable inventory system
    public PlayerInventory playerInventory;
    //public Item[] inventory = new Item[10]; // For fixed size inventory system

    public void CollectGold(int amount) 
    {
        amount = Mathf.RoundToInt(amount * DifficultyMultiplier);
        stats.AddGold(amount);
    }
    public void SpendGold(int amount)
    {
        stats.SpendGold(amount);
    }
    public void PlayDrinkSound() 
    {
        if (playerSFX != null) { playerSFX.PlayDrink(); }
    }
    public List<ItemData> GetInventory() { return playerInventory.GetInventory(); }

    public bool isClimbing
    {
        get => _isClimbing;
        set
        {
            if (_isClimbing != value)
            {
                _isClimbing = value;
                OnClimbStateChanged?.Invoke(_isClimbing);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    protected virtual void FixedUpdate()
    {
        debugVelocity = rb.linearVelocity;
    }
    protected override void Start()
    {
        base.Start();
        DifficultyMultiplier = PlayerPrefs.GetFloat("Difficulty", 1.0f);
        stats = GetComponent<PlayerStats>();
        
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
        playerInventory = GetComponent<PlayerInventory>();
       

        damageHandler = new DamageHandler(this, effectHandler);
        buffSystem = GetComponent<BuffSystem>();
        buffSystem.Initialize(this, effectHandler, true);
        elementSystem = GetComponent<ElementSystem>();

        equipment = GetComponent<PlayerEquipment>();
        equipment.Initialize(this);
        // Subscribe take damage event
        OnTakeDamage += (_, _, _, _ )=>playerAnimation.PlayHurt();
        OnTakeDamage += GetHurt;

        defaultMaterial = spriteRenderer.material;

        playerSFX = GetComponent<PlayerSFX>();

    }
    IEnumerator InitConfinerNextFrame()
    {


        var playerCamera = GameObject.Find("PlayerCamera");
        CinemachineCamera vcam = playerCamera.GetComponent<CinemachineCamera>();
        vcam.enabled = false;
        yield return null; 

        vcam.enabled = true;
        if (!vcam)
        {
            Debug.LogWarning("❌ 找不到 PlayerCamera 或 CameraBound！");
        }
       
       
    }
    public void SwitchElement() 
    { 
        elementSystem.SwitchElement();
    }
    public override void ChangeAttackSpeed(float speedMulti) { playerAnimation.ChangeAttackSpeed(speedMulti); }

    public void EquipNecklace(NecklaceSO necklaceData)
    {
        equipment.EquipNecklace(necklaceData);
    }

    public void UnequipNecklace()
    {
        equipment.UnequipNecklace();
    }

    public bool HasNecklaceEquipped()
    {
        return equipment.HasNecklaceEquipped();
    }

    public Necklace GetEquippedNecklace()
    {
        return equipment.GetEquippedNecklace();
    }

    public List<ElementType> GetAvailableElements()
    {
        return elementSystem.AvailableElements;
    }

    public int GetElementPoint(ElementType element)
    {
        return elementSystem.GetElementPoint(element);
    }

    // Replace all occurrences of "rb.velocity" with "rb.linearVelocity" to fix the CS0618 diagnostic.  
    // Example changes are shown below:

    public void SetAttackState()
    {
        rb.linearVelocity = new Vector2(0, 0); // Updated from rb.velocity
        isAttacking = true;
    }
    public void UnsetAttackState() { isAttacking = false; }
    public void UseItem(int index)
    {
        playerInventory.UseItem(this, index);
    }



    public void StoreItem(Item item, Action<bool> onComplete)
    {
        playerInventory.StoreItem(item, onComplete);
    }
    public void RemoveItem(int itemId, Action<bool> onComplete)
    {
        playerInventory.RemoveItem(itemId, onComplete);
    }


    public DamageType GetCurrentDamageType() => elementSystem.GetCurrentDamageType();
    public float GetAttackMultiplier() => buffSystem.GetAttackMultiplier();
    protected override void Update()
    {
        base.Update();

        if (isClimbing && physicsCheck.isGround)
        {
            ExitClimb();
        }

        CheckState();

        currentHealth = stats.GetCurrentHealth();
        maxHealth = stats.GetMaxHealth();
        gold = stats.GetGold();
    }
    public void Move(Vector2 inputDirection)
    {
        if (isHurt || isGroundAttack || isDashing || isAttacking) return;
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
            
            rb.linearVelocity = new Vector2(inputDirection.x * speed * speedMultiplier, rb.linearVelocity.y);
        }

        
        if (inputDirection.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (inputDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }


    public void RangeAttack()
    {
        playerAnimation.PlayerRangeAttack();
    }

    public void FireProjectile()
    {
        String attackType = "PhysicalProjectile";
        ProjectileData projectileData = projectileDatabase.GetProjectile(attackType);
        if (projectileData == null || firePoint == null) return;

        GameObject projectileInstance = Instantiate(projectileData.prefab, firePoint.position, Quaternion.identity);
        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        projectileInstance.GetComponent<Projectile>().Initialize(shootDirection, projectileData);
    }

    //public void Jump()
    //{
    //    if (physicsCheck.isGround)
    //    {
    //        rb.AddForce(transform.up* jumpForce, ForceMode2D.Impulse);
    //    }

    //}
    public void Jump()
    {
        if (physicsCheck.isGround)
        {
            // 第一次跳  
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            hasJumpedOnce = true;
            hasDoubleJumped = false; // 重置二段跳  
        }
        else if (canDoubleJump && hasJumpedOnce && !hasDoubleJumped)
        {
            // 二段跳  
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // 重置竖直速度，让二段跳手感更好  
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            hasDoubleJumped = true;
        }
    }

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }
    public void DisableDoubleJump()
    {
        canDoubleJump = false;
        hasJumpedOnce = false;
        hasDoubleJumped = false;
    }

    public int GetGold()
    {
        if (stats != null) 
        {
            return stats.GetGold();
        }
        return 0;
    }
    public IEnumerator DashCoroutine()
    {
        Debug.Log("DashCoroutine!!!");
        isDashing = true;
        canDash = false;
        TriggerInvincible(dashDuration);
        playerController.DisableInput();

        rb.linearVelocity = Vector2.zero;  // Updated from rb.velocity
        StartDashEffect();
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0); // Updated from rb.velocity

        yield return new WaitForSeconds(dashDuration);
        playerController.EnableInput();
        EndDashEffect();
        isDashing = false;
        rb.linearVelocity = Vector2.zero;  // Updated from rb.velocity

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
    public override void ModifyHP(float amount)
    {
        if (stats != null)
        {
            if (amount < 0)
                stats.ReduceHealth(-amount);
            else 
            { 
                stats.Heal(amount);
            }
        }
    }
    //public void Attack()
    //{

    //    if (EventSystem.current.IsPointerOverGameObject())
    //    {
    //        // 点击在UI上，不攻击
    //        return;
    //    }

    //    playerAnimation.PlayAttack();
    //    if (physicsCheck.isGround)
    //    {
    //        isGroundAttack = true;
    //    }
    //    if (isClimbing) 
    //    {
    //        ExitClimb();
    //    }
    //}

    public void Attack()
    {
        StartCoroutine(DelayedAttack());
    }

    private IEnumerator DelayedAttack()
    {
        yield return null; 

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            yield break; 
        }

        playerAnimation.PlayAttack();

        if (physicsCheck.isGround)
            isGroundAttack = true;

        if (isClimbing)
            ExitClimb();
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
        //currentHP = Mathf.Min(currentHP + amount, maxHP);
        stats.Heal(amount);
        OnHealthChange?.Invoke(this);
    }
    public void RestoreFullHP() 
    { 
        stats.Heal(stats.GetMaxHealth());
    }

    public void GetHurt(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        Debug.Log("Player Get Hurt");
        isHurt = true;
        //float defenceMultiplier = buffSystem.GetDefenseMultiplier();
        //damage *= (1-defenceMultiplier);
        damageHandler.HandleDamage(attacker, damage, knockbackForce, damageType);

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
        ShowDeathScreen();

    }
    public void Respawn()
    {
        if (isDead)
        {
            SaveSystem.LoadGame();
            rb.gravityScale = originalGravityScale;
            rb.linearVelocity = Vector2.zero; // Updated from rb.velocity
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerAnimation.SetClimbing(false, false);
            playerController.EnableInput();
            playerAnimation.Reset();
            isHurt = false;
            isDead = false;
            //playerAnimation.ResetTrigger();

        }
    }

    private void ShowDeathScreen() 
    {
        var ui = FindAnyObjectByType<DeathScreenController>();
        if ( ui != null)
        {
            ui.enabled = true;
            Debug.Log("Show Death Screen");
            ui.ShowDeathScreen();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (nearbyInteractable != null) return;

        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            nearbyInteractable = interactable;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            nearbyInteractable = interactable;
        }
        else if (other.CompareTag("LoadPoint"))
        {
            SaveSystem.LoadGame();
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
    public void LoadPlay() 
    {
        SaveSystem.LoadGame(); 
    }

    public override void TakeDamage(Transform attacker, float damage, float knockbackForce, DamageType damageType)
    {
        if (isInvincible || isDead) return;
        InvokeTakeDamageEvent(attacker, damage*DifficultyMultiplier, knockbackForce, damageType);
        //stats.ReduceHealth((int)damage);
        if (stats.GetCurrentHealth() <= 0)
        {
            GoldGenerator.Instance.GenerateGolds(transform.position, stats.GetGold(), true);
            stats.DieLoseGold();
            OnDie?.Invoke(transform);
        }
        //else
        //{
        //    //TriggerInvincible();
        //}

        //OnHealthChange?.Invoke(this);
    }

    //void StartDashEffect()
    //{

    //    dashEffect.gameObject.SetActive(true);
    //    dashEffect.transform.position = transform.position;
    //    if (transform.localScale.x > 0)
    //        dashEffect.transform.rotation = Quaternion.Euler(0f, 0f, 0f);   // 向右
    //    else
    //        dashEffect.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // 向左


    //}
    void StartDashEffect()
    {
        GameObject effectGO = transform.localScale.x > 0 ? dashEffectRight : dashEffectLeft;

        effectGO.transform.position = transform.position;

        currentDashEffect = effectGO.GetComponent<ParticleSystem>();

        if (currentDashEffect != null)
        {
            currentDashEffect.Clear();
            currentDashEffect.Play();
        }
    }

    void EndDashEffect()
    {
        if (currentDashEffect != null)
        {
            currentDashEffect.Stop();
        }
    }

    public float GetPlayerSpeed() 
    {
        return speed;
    }

    public void SetPlayerSpeed(float speed)
    {
        this.speed = speed;
    }

    public void ApplyBuff(BuffType type, float duration, float value = 1.0f, string sourceId="")
    {
        buffSystem.AddBuff(type, duration, value, sourceId);
    }
    public void AddElementPoint(ElementType type, int amount)
    {
        elementSystem.AddElementPoint(type, amount);
    }
    
    //public void RemoveBuff(BuffType type)
    //{
    //    buffSystem.RemoveBuff(type);
    //}
    
    public void RemoveBuffBySource(String id)
    {
        buffSystem.RemoveBuffBySource(id);
    }

    public void RemoveAllBuff()
    {
        buffSystem.RemoveAllBuff();
    }

    public void ShowRestoreEffect()
    {
        var effect = transform.Find("Effect/RestoreHealth")?.gameObject;

        if (playerSFX != null) 
        {
            playerSFX.PlayHeal();
        }
        if (effect != null)
        {
            effect.SetActive(false);
            effect.SetActive(true);
            StartCoroutine(HideEffectAfterDelay(effect, 1f));

        }

    }

    private IEnumerator HideEffectAfterDelay(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    public void DebugSuiside() 
    {
        Die();
    }

    public void PlayerSpeedUp(float percent) 
    {
        speedMultiplier += percent;
    }

    public void PlayerSpeedDown(float percent)
    {
        speedMultiplier -= percent;
    }


}
