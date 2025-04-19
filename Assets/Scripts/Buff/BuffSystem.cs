using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;
using static Unity.VisualScripting.Member;

//[System.Serializable]
//public class BuffEntry
//{
//    public BuffType type;
//    public float duration;
//}
[System.Serializable]
public class BuffEntry
{
    public BuffType type;
    public float duration;
    public float value;
    public string sourceId; 
}

//[System.Serializable]
//public class BuffSaveData
//{
//    public List<BuffEntry> activeBuffs = new();
//    public DamageType currentDamageType;
//    public float attackMultiplier;
//    public float defenseMultiplier;
//    public float attackSpeedMultiplier;
//}
[System.Serializable]
public class BuffSaveData
{
    public List<BuffEntry> activeBuffs = new();
    public DamageType currentDamageType;
}
[System.Serializable]
public class BuffInstance
{
    public BuffType type;
    public float duration;
    public float value;
    public string sourceId; 
}

public class BuffSystem : MonoBehaviour, ISaveable
{

    [SerializeField]
    private bool shouldSave = false;
    public bool ShouldSave => shouldSave;
    //private Dictionary<BuffType, float> activeBuffs = new Dictionary<BuffType, float>();
    //private Dictionary<BuffType, Buff> activeBuff = new Dictionary<BuffType, Buff>();
    [SerializeField] private List<BuffInstance> activeBuffs = new List<BuffInstance>();

    private DamageType currentDamageType = DamageType.Physical;
    
    private float attackMultiplier = 1.0f;
    private float defenseMultiplier = 1.0f;
    private float attackSpeedMultiplier = 1.0f;
    private Character target;
    private DamageEffectHandler effectHandler;
    public void Initialize(Character character, DamageEffectHandler effectHandler, bool shouldSave=false)
    {
        this.target = character;
        this.effectHandler = effectHandler;
        this.shouldSave = shouldSave;
    }
    public void AddBuff(BuffType type, float duration, float value = 1.0f, string sourceId="")
    {
        //if (type == BuffType.AttackUp) attackMultiplier += value;
        //if (type == BuffType.DefenseUp) defenseMultiplier += value;
        //if (type == BuffType.AttackSpeedUp) 
        //{ 
        //    attackSpeedMultiplier *= (1 + value);
        //    target.ChangeAttackSpeed(attackSpeedMultiplier);
        //}
        ////if (type == BuffType.FireEnchant) currentDamageType = DamageType.Fire;
        ////if (type == BuffType.IceEnchant) currentDamageType = DamageType.Ice;

        //activeBuffs[type] = duration;
        //BuffManager.instance.AddBuff(type, duration);
        var buff = new BuffInstance
        {
            type = type,
            duration = duration,
            value = value,
            sourceId = sourceId
        };

        activeBuffs.Add(buff);

        ApplyBuffEffect(buff); // 应用效果
        BuffManager.instance.AddBuff(type, duration);
    }
    private void ApplyBuffEffect(BuffInstance buff) 
    {
        if (buff.type == BuffType.AttackSpeedUp)
        {
            attackSpeedMultiplier += buff.value;
            target.ChangeAttackSpeed(attackSpeedMultiplier);
        }
        else if (buff.type == BuffType.SpeedUp)
        {
            attackSpeedMultiplier += buff.value;
            Player.Instance.PlayerSpeedUp(buff.value);
        }
        else if (buff.type == BuffType.SlowEnemy)
        {
            // Processed in SlowEnemyAbility
        }
        else if (buff.type == BuffType.DoubleJump)
        {
            Player.Instance.EnableDoubleJump();
        }
        else if (buff.type == BuffType.AttackUp)
        {
            attackMultiplier += buff.value;
        }
        else if (buff.type == BuffType.DefenseUp)
        {
            defenseMultiplier += buff.value;
        }


    }

    private void Update()
    {
        //List<BuffType> expiredBuffs = new List<BuffType>();

        //foreach (var kvp in activeBuffs.ToList())
        //{
        //    if (activeBuffs[kvp.Key] <= -1000) continue; // -9999 means infinite duration
        //    activeBuffs[kvp.Key] -= Time.deltaTime;
        //    if (activeBuffs[kvp.Key] <= 0) expiredBuffs.Add(kvp.Key);
        //}

        //foreach (var buff in expiredBuffs) RemoveBuff(buff);

        List<BuffInstance> expired = new();

        foreach (var buff in activeBuffs)
        {
            if (buff.duration > -9999)
            {
                buff.duration -= Time.deltaTime;
                if (buff.duration <= 0) expired.Add(buff);
            }
        }

        foreach (var buff in expired)
        {
            RemoveBuff(buff);
        }
    }


    //public void RemoveBuff(BuffType type)
    //{
    //    if (activeBuffs.ContainsKey(type))
    //    {
    //        activeBuffs.Remove(type);
    //        if (type == BuffType.AttackUp) attackMultiplier = 1.0f;
    //        if (type == BuffType.DefenseUp) defenseMultiplier = 1.0f;
    //        if (type == BuffType.AttackSpeedUp)
    //        {
    //            attackSpeedMultiplier = 1.0f;
    //            target.ChangeAttackSpeed(attackSpeedMultiplier);
    //        }
    //        //if (type == BuffType.FireEnchant || type == BuffType.IceEnchant) currentDamageType = DamageType.Physical;
    //        BuffManager.instance.RemoveBuff(type);

    //    }

    //}
    private void RevertBuffEffect(BuffInstance buff)
    {
        if (buff.type == BuffType.AttackSpeedUp)
        {
            attackSpeedMultiplier -= buff.value;
            target.ChangeAttackSpeed(attackSpeedMultiplier);
        }
        else if (buff.type == BuffType.SpeedUp)
        {
            attackSpeedMultiplier -= buff.value;
            Player.Instance.PlayerSpeedDown(buff.value);
        }
        else if (buff.type == BuffType.SlowEnemy)
        {
            // Processed in SlowEnemyAbility
        }
        else if (buff.type == BuffType.DoubleJump)
        {
            Player.Instance.DisableDoubleJump();
        }
        else if (buff.type == BuffType.AttackUp)
        {
            attackMultiplier -= buff.value;
        }
        else if (buff.type == BuffType.DefenseUp)
        {
            defenseMultiplier -= buff.value;
        }
    }
    public void RemoveBuff(BuffInstance buff)
    {
        RevertBuffEffect(buff);
        activeBuffs.Remove(buff);
        BuffManager.instance.RemoveBuff(buff.type);
    }
    public void RemoveBuffBySource(string sourceId)
    {
        var list = activeBuffs.Where(b => b.sourceId == sourceId).ToList();
        foreach (var b in list) RemoveBuff(b);
    }

    public void RemoveAllBuff()
    {
        activeBuffs.Clear();
        attackMultiplier = 1.0f;
        defenseMultiplier = 1.0f;
        currentDamageType = DamageType.Physical;
    }
    public DamageType GetCurrentDamageType() => currentDamageType;
    //public float GetAttackMultiplier() => attackMultiplier;
    public float GetAttackMultiplier()
    {
        float baseValue = 1f;
        foreach (var buff in activeBuffs)
        {
            if (buff.type == BuffType.AttackUp)
                baseValue += buff.value;
        }
        return baseValue;
    }
    //public float GetDefenseMultiplier() => defenseMultiplier;
    public float GetDefenseMultiplier()
    {
        float baseValue = 0f;
        foreach (var buff in activeBuffs)
        {
            if (buff.type == BuffType.DefenseUp)
                baseValue += buff.value;
        }
        return baseValue;
    }

    public string SaveKey() => "BuffSystem";


    public object CaptureState()
    {
        BuffSaveData data = new BuffSaveData
        {
            currentDamageType = currentDamageType,
        };

        foreach (var buff in activeBuffs)
        {
            data.activeBuffs.Add(new BuffEntry
            {
                type = buff.type,
                duration = buff.duration,
                value = buff.value,
                sourceId = buff.sourceId
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

        activeBuffs.Clear();
        foreach (var entry in data.activeBuffs)
        {
            var restoredBuff = new BuffInstance
            {
                type = entry.type,
                duration = entry.duration,
                value = entry.value,
                sourceId = entry.sourceId
            };

            activeBuffs.Add(restoredBuff);
            BuffManager.instance.AddBuff(entry.type, entry.duration);
        }

        target.ChangeAttackSpeed(attackSpeedMultiplier);
    }
    //public object CaptureState()
    //{
    //    BuffSaveData data = new BuffSaveData
    //    {
    //        currentDamageType = currentDamageType,
    //        attackMultiplier = attackMultiplier,
    //        defenseMultiplier = defenseMultiplier,
    //        attackSpeedMultiplier = attackSpeedMultiplier
    //    };

    //    foreach (var kv in activeBuffs)
    //    {
    //        data.activeBuffs.Add(new BuffEntry
    //        {
    //            type = kv.Key,
    //            duration = kv.Value
    //        });
    //    }

    //    return data;
    //}


    //public void RestoreState(object state)
    //{
    //    var data = state as BuffSaveData;
    //    if (data == null)
    //    {
    //        Debug.LogWarning("BuffSystem RestoreState: data transfer failed");
    //        return;
    //    }

    //    currentDamageType = data.currentDamageType;
    //    attackMultiplier = data.attackMultiplier;
    //    defenseMultiplier = data.defenseMultiplier;
    //    attackSpeedMultiplier = data.attackSpeedMultiplier;

    //    activeBuffs.Clear();
    //    foreach (var entry in data.activeBuffs)
    //    {
    //        activeBuffs[entry.type] = entry.duration;
    //    }

    //    foreach (var entry in data.activeBuffs)
    //    {
    //        BuffManager.instance.AddBuff(entry.type, entry.duration);
    //    }
    //    target.ChangeAttackSpeed(attackSpeedMultiplier);
    //}
}
