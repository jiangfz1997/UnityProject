using UnityEngine;

public class SuperconductStatusEffect : BaseStatusEffect
{
    private float damageBoostPercent;
    private bool applied = false;
    private GameObject superconductEffect;

    public SuperconductStatusEffect(Character target, float duration, float damageBoostPercent)
        : base(target, duration)
    {
        this.EffectType = StatusEffect.Superconduct;
        this.damageBoostPercent = damageBoostPercent;
    }

    public override void Apply()
    {
        if (target == null || applied) return;

        Debug.Log($"{target.name} is affected by Superconduct! Damage boost: {damageBoostPercent * 100}%");
        target.AddDamageTakenMultiplier(damageBoostPercent); 
        applied = true;


        Transform effectTransform = target.transform.Find("SuperConductEffect");
        if (effectTransform != null)
        {
            superconductEffect = effectTransform.gameObject;
            superconductEffect.SetActive(true);
        }

    }

    public override void End()
    {
        if (applied)
        {
            target.RemoveDamageTakenMultiplier(damageBoostPercent);
            applied = false;


            if (superconductEffect != null)
            {
                superconductEffect.SetActive(false);
            }
        }

        Debug.Log($"{target.name} superconduct effect ended.");
    }

    public override void Update(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (IsFinished)
        {
            End();
        }
    }
}

