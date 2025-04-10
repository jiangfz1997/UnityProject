using UnityEngine;

public class ShockWave : PlayerAttack
{
    public void DestroyShockWave()
    {
        Debug.Log("⚡ ShockWave 动画播放完毕，销毁！");
        Destroy(gameObject);
    }



    protected override void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("⚡ ShockWave 触发了 OnTriggerStay2D");
        base.OnTriggerStay2D(collision);
    }
}