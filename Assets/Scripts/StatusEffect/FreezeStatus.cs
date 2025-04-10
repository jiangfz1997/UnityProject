using UnityEngine;
using System.Collections;
public class FrozenStatus : BaseStatusEffect
{
    private float slowMultiplier;
    private float originalSpeed;
    private GameObject frozenEffect; 
    private SpriteRenderer sr;
    private Color originalColor;

    public FrozenStatus(Character target, float duration, float slowMultiplier)
        : base(target, duration)
    {
        this.EffectType = StatusEffect.Frozen;
        this.slowMultiplier = slowMultiplier;
    }

    public override void Apply()
    {
        base.Apply();
        if (target != null)
        {
            Transform effectTransform = target.transform.Find("FrozenEffect");
            if (effectTransform != null)
            {
                frozenEffect = effectTransform.gameObject;
                frozenEffect.SetActive(true);
            }
            //sr = target.GetComponentInChildren<SpriteRenderer>();
            //if (sr != null)
            //{
            //    originalColor = sr.color;
            //    target.StartCoroutine(FrozenVisualCoroutine());
            //}

            originalSpeed = target.GetCurrentSpeed();
            target.SetCurrentSpeed(originalSpeed * slowMultiplier);

            if (target.TryGetComponent<DamageEffectHandler>(out var handler))
            {
                handler.ApplyFreezeEffect(duration);
            }

            Debug.Log($"🧊 FrozenStatus applied. Speed reduced to {slowMultiplier * 100}% for {duration} seconds.");
        }
    }

    public override void Update(float deltaTime)
    {
        if (IsFinished) return;
        elapsedTime += deltaTime;

        if (IsFinished)
        {
            End();
        }
    }

    public override void End()
    {
        base.End();
        if (target != null)
        {
            target.SetCurrentSpeed(originalSpeed);
            Debug.Log($"🧊 FrozenStatus ended. Speed restored.");
        }
        if (frozenEffect != null)
        {
            frozenEffect.SetActive(false);
        }
        if (sr != null)
            sr.color = originalColor;
    }

    private IEnumerator FrozenVisualCoroutine()
    {
        float elapsed = 0f;
        SpriteRenderer bodyRenderer = target.GetComponentInChildren<SpriteRenderer>();

        while (elapsed < duration)
        {
            if (sr != null)
            {
                float alpha = Mathf.PingPong(Time.time * 3f, 0.4f) + 0.6f;
                sr.color = new Color(0.5f, 0.7f, 1f, alpha);

                // ✅ 如果角色翻转，冰冻遮罩也翻转
                if (bodyRenderer != null)
                {
                    sr.flipX = bodyRenderer.flipX;
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

}
