using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectParams
{
    public StatusEffect type;
    public float duration;
}

[System.Serializable]
public class BurningParams : StatusEffectParams
{
    public float damagePerSecond;
}

[System.Serializable]
public class FrozenParams : StatusEffectParams
{
    public float slowMultiplier;
}

[System.Serializable]
public class LightningParams : StatusEffectParams
{
    
}

[System.Serializable]
public class SuperconductParams : StatusEffectParams
{
    public float damageBoostParams;
}

public class ParalyzeParams : StatusEffectParams
{
    public float stunDuration;
}

public static class StatusEffectParamRegistry
{
    private static readonly Dictionary<StatusEffect, StatusEffectParams> effectConfigs = new()
{
    { StatusEffect.Burning, new BurningParams { type = StatusEffect.Burning, duration = 3f, damagePerSecond = 10f } },
    { StatusEffect.Frozen,  new FrozenParams { type = StatusEffect.Frozen, duration = 3f, slowMultiplier = 0.8f } },
    { StatusEffect.Lightning, new LightningParams { type = StatusEffect.Lightning, duration = 10f } },
    { StatusEffect.Paralyze, new ParalyzeParams{ type = StatusEffect.Paralyze, duration = 1f, stunDuration=0.5f} },
    { StatusEffect.Superconduct, new SuperconductParams{ type = StatusEffect.Superconduct, duration=5f, damageBoostParams=0.1f} }
};

    public static StatusEffectParams Get(StatusEffect type)
    {
        return effectConfigs.TryGetValue(type, out var param) ? param : null;
    }
}