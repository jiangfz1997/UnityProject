using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.XInput;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    private PlayerController inputController;
    private SpriteRenderer playerSprite;
    private Player player;
    private bool isCharging = false;
    private bool isChargeComplete = false;
    private bool isHoldingAttack = false; 
    private float chargeTimer = 0f;
    private float pressDuration = 0f; 
    private float minChargeTime = 0.3f; 
    public float chargeTime = 1.2f; 
    private int attackLayerIndex;
    private float normalSpeed;
    
    private GameObject shockWave;
    private GameObject movingShockWave;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
        player = GetComponent<Player>();
        inputController = GetComponent<PlayerController>();
        attackLayerIndex = anim.GetLayerIndex("AttackLayer");

        shockWave = GetComponentInChildren<ShockWave>(true)?.gameObject;
        movingShockWave = GetComponentInChildren<MovingShockWave>(true)?.gameObject;

    }

    private void OnEnable()
    {
        inputController.OnAttackPressed += StartPressing;
        inputController.OnAttackReleased += StopPressing;
    }

    private void OnDisable()
    {
        inputController.OnAttackPressed -= StartPressing;
        inputController.OnAttackReleased -= StopPressing;
    }

    private void Update()
    {
        SetAnimation();

 
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(attackLayerIndex);
        
        if (stateInfo.IsName("Attack2"))
        {
            if (isHoldingAttack)
            {
                pressDuration += Time.deltaTime;

                if (pressDuration >= minChargeTime && !isCharging)
                {
                    StartCharging();
                }
            }
        }
        if (stateInfo.IsName("Charge")) 
        {

            if (isCharging)
            {
               
                
                chargeTimer += Time.deltaTime;

                if (chargeTimer >= chargeTime && !isChargeComplete)
                {
                    isChargeComplete = true;
                    //ShowChargeEffect();
                    
                }
            }
        }

        
    }
    public void SetAnimation() 
    {

        anim.SetFloat("VelocityX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("VelocityY", rb.linearVelocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);
        anim.SetBool("isDead", player.isDead);
        anim.SetBool("isGroundAttack", player.isGroundAttack);
        anim.SetBool("isDashing", player.isDashing);

    }

    public void PlayHurt()
    {   
        //Debug.Log("Play Hurt, start animation");
        anim.SetTrigger("hurt");

    }
    private void StartPressing()
    {
        isHoldingAttack = true;
        pressDuration = 0f;
    }

    private void StopPressing()
    {
        isHoldingAttack = false;

        if (!isCharging)
        {
            //anim.SetTrigger("Attack3");
        }
        else
        {
            StopCharging(); 
        }
    }
    //public void CheckForCharge()
    //{
    //    // **此时不能用 `Input.GetMouseButton(0)`，要用 `isCharging` 判断**
    //    if (isCharging) // **如果已经检测到长按**
    //    {
    //        anim.SetTrigger("Charging"); // **进入蓄力动画**
    //    }
    //    else
    //    {
    //        anim.SetTrigger("ChargeFail"); // **如果没按住，直接进入 Attack3**
    //    }
    //}
    private void HandleAttackPressed()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack2")) // **在 Attack2 结束时检测**
        {
            StartCharging();
        }
    }
    private void HandleAttackReleased()
    {
        if (isCharging)
        {
            StopCharging(); // **松手检测**
        }
    }
    private void StartCharging()
    {
        isCharging = true;
        chargeTimer = 0;
        isChargeComplete = false;
        anim.SetBool("isCharging", true);
        anim.SetTrigger("Charging"); // **进入蓄力动画**
    }

    private void StopCharging()
    {
        isCharging = false;
        //chargeEffect.SetActive(false);
        anim.SetBool("isCharging", false);

        if (chargeTimer >= chargeTime)
        {
            anim.SetTrigger("ChargedAttack"); // **蓄力完成，进入 ChargedAttack**
        }
        else
        {
            anim.SetTrigger("ChargeFail"); // **未蓄满，进入 Attack3**
        }
    }
    public void ResetChargeFailTrigger()
    {
        anim.ResetTrigger("ChargeFail");
    }
    public void ResetChargingTrigger()
    {
        anim.ResetTrigger("Charging");
    }

    public void ResetAllChargingStats() 
    {
        ResetChargeFailTrigger();
        ResetChargingTrigger();
        isCharging = false;

    }
    public void InterruptCharge()
    {
        StopCharging(); // **受击时直接停止蓄力**
    }


    public void PlayAttack()
    {
        anim.SetTrigger("attack");

    }

    public void SetClimbing(bool isClimbing, bool isMoving)
    {
        anim.SetBool("isClimbing", isClimbing);
        anim.SetBool("isClimbMoving", isMoving);
    }

    public void PlayerRangeAttack()
    {
        anim.SetTrigger("RangeAttack");
    }
    public void ActivateShockWave()
    {
        Debug.Log("⚡ 触发冲击波！");

        // **启用 shockWave**

        // **确保它的方向跟随 Player**
        bool isFacingLeft = playerSprite.flipX;
        Vector3 scale = shockWave.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isFacingLeft ? -1 : 1); // **基于 `flipX` 翻转**
        shockWave.transform.localScale = scale;
        shockWave.SetActive(true);
        

    }

    // **✅ 在 `ChargedAttack` 动画的最后一帧触发**
    public void DeactivateShockWave()
    {
        Debug.Log("🛑 冲击波消失！");
        shockWave.SetActive(false);
    }

    public void ActivateMovingShockWave()
    {
        Debug.Log("⚡ 触发冲击波！");

        // **启用 shockWave**

        // **确保它的方向跟随 Player**
        bool isFacingLeft = playerSprite.flipX;
        Vector3 scale = movingShockWave.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isFacingLeft ? -1 : 1); // **基于 `flipX` 翻转**
        movingShockWave.transform.localScale = scale;
        movingShockWave.SetActive(true);


    }

    // **✅ 在 `ChargedAttack` 动画的最后一帧触发**
    public void DeactivateMovingShockWave()
    {
        Debug.Log("🛑 冲击波消失！");
        movingShockWave.SetActive(false);
    }

    public void ChangeAttackSpeed(float speed)
    {
        anim.SetFloat("AttackSpeed", speed);
    }
}
