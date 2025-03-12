using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    private Player player;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        SetAnimation();
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

    public void PlayAttack()
    {
        anim.SetTrigger("attack");

    }

    public void SetClimbing(bool isClimbing, bool isMoving)
    {
        anim.SetBool("isClimbing", isClimbing);
        anim.SetBool("isClimbMoving", isMoving);
    }


}
