using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class StatusEffectSystem : MonoBehaviour
{
    private Character target;
    [SerializeField]
    private List<BaseStatusEffect> activeEffects = new List<BaseStatusEffect>();

    
    public void AddEffect(BaseStatusEffect effect)
    {
        if (ElementalReactionManager.TryTriggerReaction(target, effect.EffectType, out var reaction, out var toRemove))
        {
            reaction?.Invoke(target);
            foreach (var e in toRemove)
            {
                target.statusEffectSystem.RemoveEffect(e);
            }
            return; 
        }
        var existingEffect = activeEffects.Find(e => e.EffectType == effect.EffectType);
        if (existingEffect != null)
        {
            existingEffect.Refresh(effect);
            return;
        }
        effect.Apply();
        activeEffects.Add(effect);
    }

    public void Initialize(Character owner)
    {
        this.target = owner;
    }

    public List<StatusEffect> GetActiveEffectTypes()
    {
        return activeEffects.Select(e => e.EffectType).ToList();
    }

    private void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            effect.Update(Time.deltaTime);
            if (effect.IsFinished)
            {
                effect.End();
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void RemoveEffect(StatusEffect type)
    {
        
        var effect = activeEffects.Find(e => e.EffectType == type);
        if (effect != null)
        {
            effect.End();
            activeEffects.Remove(effect);
        }
    }
}
