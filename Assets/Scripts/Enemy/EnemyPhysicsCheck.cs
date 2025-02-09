using UnityEngine;

public class EnemyPhysicsCheck : PhysicsCheck
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public bool isCliffAhead;

    public Vector2 cliffCheckOffset;

    override protected void Awake()
    {
        base.Awake();
        float direction = -transform.localScale.x > 0 ? 1:-1;
    }

    override public void Check()
    {
        base.Check();
        float direction = transform.localScale.x > 0 ? 1 : -1;
        Vector2 adjustedCliffCheckOffset = new Vector2(cliffCheckOffset.x * direction, cliffCheckOffset.y);
        Vector2 cliffCheckPosition = (Vector2)transform.position + adjustedCliffCheckOffset;
        isCliffAhead = !Physics2D.Raycast(cliffCheckPosition, Vector2.down, 0.5f, groundLayer);
        Debug.DrawRay(cliffCheckPosition, Vector2.down * 0.5f, Color.green);
    }

    override protected void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireSphere((Vector2)transform.position + cliffCheckOffset, checkRaduis);
    }

    // Update is called once per frame
    override protected void Update()
    {
        Check();
        
    }


}
