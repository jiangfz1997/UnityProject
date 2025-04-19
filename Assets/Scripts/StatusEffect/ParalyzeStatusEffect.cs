using UnityEngine;

public class ParalyzeStatusEffect : BaseStatusEffect
{
    private GameObject paralyzeEffect;

    public ParalyzeStatusEffect(Character target, float duration) : base(target, duration)
    {
        this.EffectType = StatusEffect.Paralyze;
    }

    public override void Apply()
    {
        Debug.Log($"{target.name} is paralyzed for {duration} seconds!");

        target.SetParalyzed(true);

        Transform effectTransform = target.transform.Find("ParalyzeEffect");
        if (effectTransform != null)
        {
            paralyzeEffect = effectTransform.gameObject;
            paralyzeEffect.SetActive(true);
        }
    }

    public override void End()
    {
        Debug.Log($"{target.name} paralyze ended.");

        target.SetParalyzed(false);

        if (paralyzeEffect != null)
        {
            paralyzeEffect.SetActive(false);
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
