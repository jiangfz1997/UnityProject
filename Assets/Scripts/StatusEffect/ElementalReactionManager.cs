using System;
using System.Collections.Generic;
using UnityEngine;

public static class ElementalReactionManager
{
    private static readonly Dictionary<Tuple<StatusEffect, StatusEffect>, Action<Character>> elementalReactions =
        new Dictionary<Tuple<StatusEffect, StatusEffect>, Action<Character>>
        {
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Burning, StatusEffect.Frozen), ApplyMeltEffect },
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Frozen, StatusEffect.Burning), ApplyMeltEffect },
            //{ new Tuple<StatusEffect, StatusEffect>(StatusEffect.Electrified, StatusEffect.Frozen), ApplySuperconductEffect }
            
        };

  

    public static bool TryTriggerReaction(
        Character target,
        StatusEffect newEffect,
        out Action<Character> reaction,
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


    private static void ApplyMeltEffect(Character target)
    {
       
        target.ModifyHP(-15); 
        Transform effectTransform = target.transform.Find("meltEffect");
        if (effectTransform != null)
        {
            effectTransform.gameObject.SetActive(true);

        }
    }

    
    //private static void ApplySuperconductEffect(Character target)
    //{
    //   
    //    target.GetComponent<BuffSystem>().AddBuff(BuffType.DefenseDown, 5f, 0.8f);
    //}
}
