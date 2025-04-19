using UnityEngine;

public class MirrorAnim : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    public Animator animator;

    private Transform playerTransform;
    private bool isAttacking = false;
    void Start()
    {
        laser.SetActive(false);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        if (!isAttacking)
        {
            FacePlayer();
        }
    }

  private void FacePlayer()
    {
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;
        Vector3 newScale = transform.localScale;
        newScale.x = shouldFaceRight ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }

    public void Cast()
    {
        RotateLaser();
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    private void RotateLaser()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 startPosition = transform.position + new Vector3(-0.73f, 0, 0);
        laser.transform.position = startPosition;

        Vector3 direction;
        if (playerPosition.x < startPosition.x)
        {
            direction = startPosition - playerPosition;
        }
        else
        {
           direction = playerPosition - startPosition;
        }
        // Vector3 direction = playerPosition - startPosition;
        // Vector3 direction = startPosition - playerPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void LaunchLaser()
    {
        laser.SetActive(true);
    }

    public void AttackFinish()
    {
        Destroy(gameObject);
    }
}

