using UnityEngine;

public class TreasureChestAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayOpenAnimation()
    {
        animator.SetTrigger("Open");
    }
}

