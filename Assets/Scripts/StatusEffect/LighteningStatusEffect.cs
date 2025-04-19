using UnityEngine;


// Deprecated: This class is not used for current game mechanics. Only saved for possible future use. 
public class LightningStatus : BaseStatusEffect
{
    private int hitCount = 1;
    private GameObject lightningEffect;

    public LightningStatus(Character target) : base(target, 10f)
    {
        this.EffectType = StatusEffect.Lightning;
    }

    public override void Apply()
    {
        Debug.Log($"{target.name} is affected by lightning.");
        Transform effectTransform = target.transform.Find("LightningEffect");
        if (effectTransform != null)
        {
            lightningEffect = effectTransform.gameObject;
            lightningEffect.SetActive(true);
        }
        //if (target.TryGetComponent<FlashEffect>(out var flash)) { flash.Flash(Color.yellow, 0.3f); }
    }

    public override void Refresh(BaseStatusEffect newEffect)
    {
        base.Refresh(newEffect);
        //if (target.TryGetComponent<FlashEffect>(out var flash)) { flash.Flash(Color.yellow, 0.3f); }

        //hitCount++;
        //Debug.Log($"{target.name} lightning stack: {hitCount}/3");

        //if (hitCount >= 3)
        //{

        //    target.statusEffectSystem.AddEffect(new ParalyzeStatusEffect(target, 0.5f), 0);

        //    End();
        //    target.statusEffectSystem.RemoveEffect(this.EffectType);
        //}
    }

    public override void End()
    {
        Debug.Log($"{target.name} lightning status ended.");
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(false);
        }
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
