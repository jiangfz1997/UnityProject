using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerInputController inputControl;
    private SpriteRenderer spriteRenderer;
    private Material hurtMaterial;
    public Vector2 inputDirection;
    public float speed;
    public float jumpForce;
    [Header("Damage Attributes")]
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;

    public float hurtForce;
    public bool isHurt;
    public bool isDead;
    public bool isGroundAttack;
    [Header("PhysicMeterial")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputController();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hurtMaterial = spriteRenderer.material;
        playerAnimation = GetComponent<PlayerAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        GetComponent<SpriteRenderer>().material.SetFloat("_FlashControl", 1);

        // Jump
        inputControl.Player.Jump.started += Jump; 

        inputControl.Player.Attack.started += Attack;

    }



    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        if (inputControl != null) {
            inputDirection = inputControl.Player.Move.ReadValue<Vector2>();
            CheckState();
        }
    }

    private void FixedUpdate()
    {
        if (isHurt || isGroundAttack)
        {
            return;
        }
        Move();
    }

    public void Move() {
        
        rb.linearVelocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.linearVelocity.y);

        //flip the character
        int faceDirection = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDirection = 1;
        if (inputDirection.x < 0)
            faceDirection = -1;
        transform.localScale = new Vector3(faceDirection, 1, 1);
        
    }

    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("JUMP");
        if (physicsCheck.isGround) { 
            rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
        }
        
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");
        playerAnimation.PlayAttack();
        if (physicsCheck.isGround) 
        {
            isGroundAttack = true;
        }

    }

    #region UnityEvents
    public void GetHurt(Transform attacker)
    {
        Debug.Log("Get Hurt");
        isHurt = true;
        rb.linearVelocity = Vector2.zero; 

        Vector2 playerPosition = transform.position;
        Vector2 attackerPosition = attacker.position;


        Vector2 forceDirection = (playerPosition - attackerPosition).normalized;


        if (forceDirection == Vector2.zero)
        {
            forceDirection = new Vector2(0.1f, 0.1f).normalized;
        }

        Vector2 force = forceDirection * hurtForce;


        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(HurtEffect(spriteRenderer, spriteRenderer.material));
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Player.Disable();
        
    }
    #endregion
    IEnumerator HurtEffect(SpriteRenderer sr, Material mat)
    {
        float totalDuration = 2f; 
        float redDuration = 0.5f; 
        float elapsed = 0f;

        mat.SetFloat("_HurtIntensity", 1); 

        while (elapsed < totalDuration)
        {
            elapsed += Time.deltaTime;

            
            float redT = Mathf.Clamp01(elapsed / redDuration);
            mat.SetFloat("_HurtIntensity", Mathf.Lerp(1, 0, redT));

           
            float flashT = Mathf.PingPong(elapsed * 2f, 1);
            mat.SetFloat("_FlashControl", flashT);

            yield return null;
        }

        mat.SetFloat("_HurtIntensity", 0);
        mat.SetFloat("_FlashControl", 1);
    }

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
    public void DestoryCollider()
    {
        coll.enabled = false;
    }

}
