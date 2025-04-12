using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;
    private Player player;
    private Vector2 moveInput;
    private float climbInput;
    private InputAction attackAction;
    public delegate void AttackAction();
    public event AttackAction OnAttackPressed;
    public event AttackAction OnAttackReleased;

    private void Awake()
    {
        player = GetComponent<Player>();
        inputControl = new PlayerInputController();

        // bind input events
        inputControl.Player.Jump.started += Jump;
        inputControl.Player.Attack.started += Attack;
        inputControl.Player.RangeAttack.started += RangeAttack;

        inputControl.Player.Interact.started += OnInteract;
        inputControl.Player.Move.performed += OnMove;
        inputControl.Player.Move.canceled += OnMove;

        inputControl.Player.Dash.started += Dash;
        inputControl.Player.SwitchElement.started += SwitchElement;
        inputControl.Player.UseItem.performed += ctx => OnUseItem(ctx);
        attackAction = inputControl.Player.Attack;

        inputControl.Player.DebugSuiside.started += DebugSuiside;
        //DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(transform.GetChild(0).gameObject);
    }

    private void OnEnable()
    {
        attackAction.started += ctx => OnAttackPressed?.Invoke(); 
        attackAction.canceled += ctx => OnAttackReleased?.Invoke();
        inputControl.Enable();
        Player.OnClimbStateChanged += HandleClimbStateChanged;
    }

    private void OnDisable() 
    {
        attackAction.started -= ctx => OnAttackPressed?.Invoke();
        attackAction.canceled -= ctx => OnAttackReleased?.Invoke();
        inputControl.Disable();
        Player.OnClimbStateChanged -= HandleClimbStateChanged;

    }
    private void HandleClimbStateChanged(bool isClimbing)
    {
        //if (isClimbing)
        //{
        //    gameObject.layer = LayerMask.NameToLayer("ClimbingLadder"); 
        //}
        //else
        //{
        //    gameObject.layer = LayerMask.NameToLayer("Player"); 
        //}
        Debug.Log("ClimbStateChanged");
        // TODO: changing layer can cause some issues, player can be stucked into the wall , need to find a better way to handle this
    }
    private void Update()
    {
        if (inputControl != null)
        {
            //Vector2 moveInput = inputControl.Player.Move.ReadValue<Vector2>();

            player.Move(moveInput);
            if (player.CanClimb())
            {
                player.Climb(climbInput);
            }
        }
    }
    private void DebugSuiside(InputAction.CallbackContext context) => player.DebugSuiside();
    private void Jump(InputAction.CallbackContext context) => player.Jump();
    private void Attack(InputAction.CallbackContext context) => player.Attack();
    private void OnInteract(InputAction.CallbackContext context) => player.Interact();

    private void RangeAttack(InputAction.CallbackContext context) => player.RangeAttack();
    private void Dash(InputAction.CallbackContext context) => player.Dash();

    private void SwitchElement(InputAction.CallbackContext context) => player.SwitchElement();
    //private void OnUseItem(InputAction.CallbackContext context) => player.UseItem();
    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("Move Input: " + context.ReadValue<Vector2>());
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector2(input.x, 0); 
        climbInput = input.y; 
    }
    private void OnUseItem(InputAction.CallbackContext context)
    {
        string key = context.control.name; 

        if (int.TryParse(key, out int index))
        {
            index -= 1;
            player.UseItem(index);
        }
    }

    public void DisableInput()
    {
        inputControl.Player.Disable();
    }

    public void EnableInput()
    {
        inputControl.Player.Enable();
    }

}
