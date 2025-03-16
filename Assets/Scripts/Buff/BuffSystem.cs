using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public class BuffSystem : MonoBehaviour
{


    private Dictionary<BuffType, float> activeBuffs = new Dictionary<BuffType, float>();
    private DamageType currentDamageType = DamageType.Physical;
    private Dictionary<StatusEffect, float> activeStatusEffects = new Dictionary<StatusEffect, float>(); // ✅ 存储状态 Buff

    private float attackMultiplier = 1.0f;
    private float defenseMultiplier = 1.0f;
    private Character target;
    private DamageEffectHandler effectHandler;
    public void Initialize(Character character, DamageEffectHandler effectHandler)
    {
        this.target = character;
        this.effectHandler = effectHandler;
    }
    public void AddBuff(BuffType type, float duration, float value = 1.0f)
    {
        if (type == BuffType.AttackUp) attackMultiplier += value;
        if (type == BuffType.DefenseUp) defenseMultiplier += value;
        if (type == BuffType.FireEnchant) currentDamageType = DamageType.Fire;
        if (type == BuffType.IceEnchant) currentDamageType = DamageType.Ice;
        //if (type == BuffType.Burning)
        //{
        //    if (!activeBuffs.ContainsKey(BuffType.Burning))  // 避免重复燃烧
        //    {
        //        target.StartCoroutine(ApplyBurningEffect(duration, value));
        //    }
        //}

        //if (type == BuffType.Frozen)
        //{
        //    if (!activeBuffs.ContainsKey(BuffType.Frozen))
        //    {
        //        target.StartCoroutine(ApplyFrozenEffect(duration, value));
        //    }
        //}
        activeBuffs[type] = duration;
    }

    public void AddStatus(StatusEffect status, float duration, float value = 1.0f)
    {
        if (ElementalReactionManager.TryTriggerReaction(GetComponent<Character>(), status, out Action<Character> reaction))
        {
            // ✅ 触发元素反应
            reaction.Invoke(GetComponent<Character>());
            return; // **元素反应触发，不再添加状态**
        }
        if (activeStatusEffects.ContainsKey(status))
        {
            activeStatusEffects[status] = Mathf.Max(activeStatusEffects[status], duration);
        }
        else
        {
            activeStatusEffects[status] = duration;

            if (status == StatusEffect.Burning) StartCoroutine(ApplyBurningEffect(3, 5));
            if (status == StatusEffect.Frozen) StartCoroutine(ApplyFrozenEffect(3,0.8f)); // TODO: fix the hard code!!
        }
    }

    public void RemoveStatus(StatusEffect status)
    {
        if (activeStatusEffects.ContainsKey(status))
        {
            activeStatusEffects.Remove(status);
        }
    }

    private IEnumerator ApplyBurningEffect(float duration, float damagePerSecond)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.ModifyHP(-damagePerSecond);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        RemoveStatus(StatusEffect.Burning);
    }

    private IEnumerator ApplyFrozenEffect(float duration, float slowMultiplier)
    {
        float originalSpeed = target.GetCurrentSpeed(); // 记录原速度
        target.SetCurrentSpeed(slowMultiplier * originalSpeed); // **降低速度**

        effectHandler.ApplyFreezeEffect(duration);

        yield return new WaitForSeconds(duration);

        target.SetCurrentSpeed(originalSpeed); // **恢复速度**
        RemoveStatus(StatusEffect.Frozen);
    }

    public Dictionary<StatusEffect, float> GetActiveStatuses() 
    {
        return activeStatusEffects;
    }
    private void Update()
    {
        List<BuffType> expiredBuffs = new List<BuffType>();

        // ✅ 先创建一个副本，防止修改时抛出异常
        foreach (var kvp in activeBuffs.ToList())
        {
            activeBuffs[kvp.Key] -= Time.deltaTime;
            if (activeBuffs[kvp.Key] <= 0) expiredBuffs.Add(kvp.Key);
        }

        foreach (var buff in expiredBuffs) RemoveBuff(buff);
    }


    public void RemoveBuff(BuffType type)
    {
        if (activeBuffs.ContainsKey(type))
        {
            activeBuffs.Remove(type);
            if (type == BuffType.AttackUp) attackMultiplier = 1.0f;
            if (type == BuffType.DefenseUp) defenseMultiplier = 1.0f;
            if (type == BuffType.FireEnchant || type == BuffType.IceEnchant) currentDamageType = DamageType.Physical;
        }
        
    }
    
    public void RemoveAllBuff()
    {
        activeBuffs.Clear();
        attackMultiplier = 1.0f;
        defenseMultiplier = 1.0f;
        currentDamageType = DamageType.Physical;
    }
    public DamageType GetCurrentDamageType() => currentDamageType;
    public float GetAttackMultiplier() => attackMultiplier;
    public float GetDefenseMultiplier() => defenseMultiplier;
}
