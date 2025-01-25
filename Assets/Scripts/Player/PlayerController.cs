using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerInputController inputControl;
    public Vector2 inputDirection;
    public float speed;
    public float jumpForce;
    [Header("基本参数")]
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    public float hurtForce;
    public bool isHurt;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputController();

        inputControl.Player.Jump.started += Jump; // 把jump方法添加到对应事件执行
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

        }
    }

    private void FixedUpdate()
    {
        if (isHurt)
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

    public void GetHurt(Transform attacker)
    {
        //Debug.Log("Get Hurt");
        isHurt = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce((transform.position - attacker.position).normalized * hurtForce, ForceMode2D.Impulse);

    }
}
