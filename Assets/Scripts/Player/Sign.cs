using UnityEngine;

public class Sign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject signSprite;
    public Transform playerTrans;
    private bool canPress;

    private Animator animator;

    private void Awake()
    {
        animator = signSprite.GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            //animator.SetBool("canPress", canPress);
        }
        else
        {
            canPress = false;
            //animator.SetBool("canPress", canPress);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Interactable"))
        //{
        canPress = false;
            //animator.SetBool("canPress", canPress);
        //}
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }
}
