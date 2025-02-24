using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;
    private Player player;
    private Vector2 moveInput;
    private float climbInput; // **存储垂直输入**

    private void Awake()
    {
        player = GetComponent<Player>();
        inputControl = new PlayerInputController();

        // bind input events
        inputControl.Player.Jump.started += Jump;
        inputControl.Player.Attack.started += Attack;
        inputControl.Player.Interact.started += OnInteract;
        inputControl.Player.Move.performed += OnMove;
        inputControl.Player.Move.canceled += OnMove;
    }

    private void OnEnable()
    {
        inputControl.Enable();
        Player.OnClimbStateChanged += HandleClimbStateChanged;
    }

    private void OnDisable() 
    { 
        inputControl.Disable();
        Player.OnClimbStateChanged -= HandleClimbStateChanged;

    }
    private void HandleClimbStateChanged(bool isClimbing)
    {
        if (isClimbing)
        {
            gameObject.layer = LayerMask.NameToLayer("ClimbingLadder"); // **进入爬梯模式**
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player"); // **恢复正常状态**
        }
    }
    private void Update()
    {
        if (inputControl != null)
        {
            //Vector2 moveInput = inputControl.Player.Move.ReadValue<Vector2>();

            player.Move(moveInput);
            if (player.CanClimb())
            {
                player.Climb(climbInput); // **持续执行爬梯**
            }
        }
    }

    private void Jump(InputAction.CallbackContext context) => player.Jump();
    private void Attack(InputAction.CallbackContext context) => player.Attack();
    private void OnInteract(InputAction.CallbackContext context) => player.Interact();

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector2(input.x, 0); // **只记录水平移动**
        climbInput = input.y; // **存储垂直移动**
    }


    public void DisableInput()
    {
        inputControl.Player.Disable();
    }

}
