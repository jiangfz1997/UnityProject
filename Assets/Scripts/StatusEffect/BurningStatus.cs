using UnityEngine;


public class BurningStatus : BaseStatusEffect
{
    private float damagePerSecond;
    private float tickTimer = 0f; 
    private GameObject burningEffect;

    public BurningStatus(Character target, float duration, float damagePerSecond)
        : base(target, duration)
    {
        this.damagePerSecond = damagePerSecond;
        this.EffectType = StatusEffect.Burning;
    }


    public override void Apply()
    {
        Debug.Log($"{target.name} burning！last {duration} sec， {damagePerSecond} dps。");
        if (target != null)
        {
            Transform effectTransform = target.transform.Find("BurningEffect");
            if (effectTransform != null)
            {
                burningEffect = effectTransform.gameObject;
                burningEffect.SetActive(true);
            }
        }

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

        if (tickTimer >= 1f)
        {
            tickTimer = 0f;

            if (target != null)
            {
                target.ModifyHP(-damagePerSecond);
            }
        }

        if (IsFinished)
        {
            End();
        }
    }


    public override void End()
    {
        Debug.Log($"{target.name} buring end!");
        if (burningEffect != null)
        {
            burningEffect.SetActive(false);
        }

        Debug.Log("BurningStatus ended.");
    }
}

