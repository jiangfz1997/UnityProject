using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

[System.Serializable]
public class BuffEntry
{
    public BuffType type;
    public float duration;
}

[System.Serializable]
public class BuffSaveData
{
    public List<BuffEntry> activeBuffs = new();
    public DamageType currentDamageType;
    public float attackMultiplier;
    public float defenseMultiplier;
    public float attackSpeedMultiplier;
}


public class BuffSystem : MonoBehaviour, ISaveable
{


    private Dictionary<BuffType, float> activeBuffs = new Dictionary<BuffType, float>();
    //private Dictionary<BuffType, Buff> activeBuff = new Dictionary<BuffType, Buff>();
    private DamageType currentDamageType = DamageType.Physical;
    
    private float attackMultiplier = 1.0f;
    private float defenseMultiplier = 1.0f;
    private float attackSpeedMultiplier = 1.0f;
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
        if (type == BuffType.AttackSpeedUp) 
        { 
            attackSpeedMultiplier *= (1 + value);
            target.ChangeAttackSpeed(attackSpeedMultiplier);
        }
        //if (type == BuffType.FireEnchant) currentDamageType = DamageType.Fire;
        //if (type == BuffType.IceEnchant) currentDamageType = DamageType.Ice;

        activeBuffs[type] = duration;
        BuffManager.instance.AddBuff(type, duration);
    }

    private void Update()
    {
        List<BuffType> expiredBuffs = new List<BuffType>();

        foreach (var kvp in activeBuffs.ToList())
        {
            if (activeBuffs[kvp.Key] <= -1000) continue; // -9999 means infinite duration
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
            if (type == BuffType.AttackSpeedUp)
            {
                attackSpeedMultiplier = 1.0f;
                target.ChangeAttackSpeed(attackSpeedMultiplier);
            }
            //if (type == BuffType.FireEnchant || type == BuffType.IceEnchant) currentDamageType = DamageType.Physical;
            BuffManager.instance.RemoveBuff(type);

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

    public string SaveKey() => "BuffSystem";

    public object CaptureState()
    {
        BuffSaveData data = new BuffSaveData
        {
            currentDamageType = currentDamageType,
            attackMultiplier = attackMultiplier,
            defenseMultiplier = defenseMultiplier,
            attackSpeedMultiplier = attackSpeedMultiplier
        };

        foreach (var kv in activeBuffs)
        {
            data.activeBuffs.Add(new BuffEntry
            {
                type = kv.Key,
                duration = kv.Value
            });
        }

        return data;
    }


    public void RestoreState(object state)
    {
        var data = state as BuffSaveData;
        if (data == null)
        {
            Debug.LogWarning("BuffSystem RestoreState: data transfer failed");
            return;
        }

        currentDamageType = data.currentDamageType;
        attackMultiplier = data.attackMultiplier;
        defenseMultiplier = data.defenseMultiplier;
        attackSpeedMultiplier = data.attackSpeedMultiplier;

        activeBuffs.Clear();
        foreach (var entry in data.activeBuffs)
        {
            activeBuffs[entry.type] = entry.duration;
        }

        foreach (var entry in data.activeBuffs)
        {
            BuffManager.instance.AddBuff(entry.type, entry.duration);
        }

        target.ChangeAttackSpeed(attackSpeedMultiplier);
    }
}
