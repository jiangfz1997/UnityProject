using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public Vector2 bottomOffset; // The offset of the raycast from the center of the object

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isGround;
    public float checkRaduis;
    public LayerMask groundLayer;
    private void Update()
    {
        Check();
    }

    public void Check()
    { 
        //ºÏ≤‚µÿ√Ê
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
    }
}
