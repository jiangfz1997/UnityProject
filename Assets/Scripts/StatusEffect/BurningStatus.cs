using UnityEngine;


public class BurningStatus : BaseStatusEffect
{
    private float damagePerSecond;
    private float tickTimer = 0f; // 用来记录每次间隔
    private GameObject burningEffect; // 🔥火焰特效对象（需挂在角色身上）

    public BurningStatus(Character target, float duration, float damagePerSecond)
        : base(target, duration)
    {
        this.damagePerSecond = damagePerSecond;
        this.EffectType = StatusEffect.Burning;
    }


    public override void Apply()
    {
        Debug.Log($"🔥 {target.name} 开始灼烧！持续 {duration} 秒，每秒 {damagePerSecond} 点伤害。");
        if (target != null)
        {
            Transform effectTransform = target.transform.Find("BurningEffect");
            if (effectTransform != null)
            {
                burningEffect = effectTransform.gameObject;
                burningEffect.SetActive(true);
            }
        }

        Debug.Log("🔥 BurningStatus applied.");
    }
    public override void Refresh(BaseStatusEffect newEffect)
    {
        base.Refresh(newEffect);

        if (newEffect is BurningStatus burning)
        {
            this.damagePerSecond = burning.damagePerSecond; 
        }
    }

    public override void Update(float deltaTime)
    {
        if (IsFinished) return;

        elapsedTime += deltaTime;
        tickTimer += deltaTime;

        if (tickTimer >= 1f) // 每 1 秒触发一次
        {
            tickTimer = 0f;

            if (target != null)
            {
                target.ModifyHP(-damagePerSecond); // 每秒一次
            }
        }

        if (IsFinished)
        {
            End();
        }
    }


    public override void End()
    {
        Debug.Log($"🔥 {target.name} 的灼烧效果结束。");
        if (burningEffect != null)
        {
            burningEffect.SetActive(false);
        }

        Debug.Log("🔥 BurningStatus ended.");
        // 如果需要，这里可以执行额外清理，例如播放结束特效
    }
}

