using System;
using System.Collections.Generic;
using UnityEngine;

public static class ElementalReactionManager
{
    // 🔥 **元素反应表**
    private static readonly Dictionary<Tuple<StatusEffect, StatusEffect>, Action<Character>> elementalReactions =
        new Dictionary<Tuple<StatusEffect, StatusEffect>, Action<Character>>
        {
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Burning, StatusEffect.Frozen), ApplyMeltEffect },
            { new Tuple<StatusEffect, StatusEffect>(StatusEffect.Frozen, StatusEffect.Burning), ApplyMeltEffect },
            //{ new Tuple<StatusEffect, StatusEffect>(StatusEffect.Electrified, StatusEffect.Frozen), ApplySuperconductEffect }
            // ✅ 你可以继续添加新的反应，比如 "火+雷=超载"
        };

    public static bool TryTriggerReaction(Character target, StatusEffect newEffect, out Action<Character> reaction)
    {
        foreach (var effect in target.GetComponent<BuffSystem>().GetActiveStatuses().Keys)
        {
            var key = new Tuple<StatusEffect, StatusEffect>(effect, newEffect);
            if (elementalReactions.TryGetValue(key, out reaction))
            {
                return true;
            }
        }

        reaction = null;
        return false;
    }

    // ✅ 反应 1：融化（🔥+❄）
    private static void ApplyMeltEffect(Character target)
    {
        Debug.Log("🔥❄ 融化反应触发，造成额外伤害！");
        target.ModifyHP(-15); // 直接扣额外伤害
    }

    //// ✅ 反应 2：超导（⚡+❄）
    //private static void ApplySuperconductEffect(Character target)
    //{
    //    Debug.Log("⚡❄ 超导触发，减少目标防御力！");
    //    target.GetComponent<BuffSystem>().AddBuff(BuffType.DefenseDown, 5f, 0.8f);
    //}
}
