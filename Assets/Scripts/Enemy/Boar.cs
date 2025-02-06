using UnityEngine;

public class Boar : Enemy
{
    override public void Move()
    {
        base.Move();
        anim.SetBool("walk", true);
    }

}
