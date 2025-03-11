using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;
    private Player player;
    private Vector2 moveInput;
    private float climbInput;

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
        inputControl.Player.Dash.started += Dash;
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
                player.Climb(climbInput); // **³ÖÐøÖ´ÐÐÅÀÌÝ**
            }
        }
    }

    private void Jump(InputAction.CallbackContext context) => player.Jump();
    private void Attack(InputAction.CallbackContext context) => player.Attack();
    private void OnInteract(InputAction.CallbackContext context) => player.Interact();

    private void Dash(InputAction.CallbackContext context) => player.Dash();
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector2(input.x, 0); 
        climbInput = input.y; 
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
