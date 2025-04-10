using UnityEngine;

public class MovingShockWave : PlayerAttack
{
    public void DestroyShockWave()
    {
        gameObject.SetActive(false);
    }
    



    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }
}