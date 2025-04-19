using System;
using System.Collections.Generic;
using UnityEngine;

public static class ElementalReactionManager
{
    private static readonly Dictionary<Tuple<StatusEffect, StatusEffect>, Action<Character,float>> elementalReactions =
        new Dictionary<Tuple<StatusEffect, StatusEffect>, Action<Character,float>>
        {
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Burning, StatusEffect.Frozen), ApplyMeltEffect },
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Frozen, StatusEffect.Burning), ApplyMeltEffect },
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Lightning, StatusEffect.Burning), ApplyOverHeatEffect },
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Burning, StatusEffect.Lightning), ApplyOverHeatEffect },
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Frozen, StatusEffect.Lightning), ApplySuperconductEffect },
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Lightning, StatusEffect.Frozen), ApplySuperconductEffect },

        };

  

    public static bool TryTriggerReaction(
        Character target,
        float damage,
        StatusEffect newEffect,
        out Action<Character, float> reaction,
        out List<StatusEffect> elementsToRemove)
    {
        elementsToRemove = new List<StatusEffect>();

    
        var statusSystem = target.statusEffectSystem;
        if (statusSystem == null)
        {
            reaction = null;
            return false;
        }

        foreach (var effect in statusSystem.GetActiveEffectTypes())
        {
            var key = new Tuple<StatusEffect, StatusEffect>(effect, newEffect);
            if (elementalReactions.TryGetValue(key, out reaction))
            {
                elementsToRemove.Add(effect);       
                elementsToRemove.Add(newEffect);   
                return true;
            }
        }

        reaction = null;
        return false;
    }


    private static void ApplyMeltEffect(Character target, float damage)
    {
        float meltDamage = -1f * damage * 0.5f;
        target.ModifyHP(meltDamage); 
        Transform effectTransform = target.transform.Find("meltEffect");
        if (effectTransform != null)
        {
            effectTransform.gameObject.SetActive(true);

        }
    }


    private static void ApplySuperconductEffect(Character target, float damage)
    {
        var effect = StatusEffectFactory.CreateEffect(StatusEffect.Superconduct, target);
        if (effect != null)
        {
            target.statusEffectSystem.AddEffect(effect, damage);
        }
    }

    private static void ApplyOverHeatEffect(Character target, float damage)
    {
        Transform effectTransform = target.transform.Find("OverheatEffect");
        if (effectTransform != null)
        {
            effectTransform.gameObject.SetActive(true);
        }
    }
}
