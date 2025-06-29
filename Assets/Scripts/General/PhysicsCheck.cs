﻿using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    protected CapsuleCollider2D coll;
    public Vector2 bottomOffset; // The offset of the raycast from the center of the object

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isGround;

    public float checkRaduis;
    public LayerMask groundLayer;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool manual;

    private GameObject player;

    private Transform movingPlatform = null;
    private Vector3 lastPlatformPosition;


    protected virtual void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");

        if (!manual)
        {
            rightOffset = new Vector2(coll.bounds.size.x / 2 + coll.offset.x, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
            Debug.Log($"Right offset is: {rightOffset}, Left offset is: {leftOffset}");
        }

        player = GameObject.FindWithTag("Player");
    }
    protected virtual void Update()
    {
        Check();
    }

    public virtual void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);
        touchLeftWall = Physics2D.Raycast((Vector2)transform.position + leftOffset, Vector2.left, 0.5f, groundLayer);
        touchRightWall = Physics2D.Raycast((Vector2)transform.position + rightOffset, Vector2.right, 0.5f, groundLayer);


        Debug.DrawRay((Vector2)transform.position + leftOffset, Vector2.left * 0.5f, Color.red);
        Debug.DrawRay((Vector2)transform.position + rightOffset, Vector2.right * 0.5f, Color.blue);

    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (bottomOffset == null || leftOffset == null || rightOffset == null)
        {
            Debug.LogWarning("One of the offsets is null!");
            return;
        }
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform") && isGround)
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y > 0.5f)
            {
                movingPlatform = collision.transform;
                lastPlatformPosition = movingPlatform.position;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform") && movingPlatform == collision.transform)
        {
            movingPlatform = null;
        }
    }

    void LateUpdate()
    {
        if (movingPlatform != null)
        {
            Vector3 delta = movingPlatform.position - lastPlatformPosition;
            transform.position += delta;
            lastPlatformPosition = movingPlatform.position;
        }
    }


}
