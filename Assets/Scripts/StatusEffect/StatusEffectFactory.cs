using UnityEngine;



public static class StatusEffectFactory
{
    public static BaseStatusEffect CreateEffect(StatusEffect type, Character target)
    {
        StatusEffectParams baseParam = StatusEffectParamRegistry.Get(type);

        if (baseParam == null) return null;

        switch (type)
        {
            case StatusEffect.Burning:
                if (baseParam is BurningParams burn)
                    return new BurningStatus(target, burn.duration, burn.damagePerSecond);
                break;

            case StatusEffect.Frozen:
                if (baseParam is FrozenParams freeze)
                    return new FrozenStatus(target, freeze.duration, freeze.slowMultiplier);
                break;
            
            case StatusEffect.Lightning:
                if (baseParam is LightningParams lightning)
                    return new LightningStatus(target);
                break;
            case StatusEffect.Paralyze:
                if (baseParam is ParalyzeParams paralyze)
                    return new ParalyzeStatusEffect(target, paralyze.stunDuration);
                break;
            case StatusEffect.Superconduct:
                if (baseParam is SuperconductParams superconduct)
                    return new SuperconductStatusEffect(target, superconduct.duration, superconduct.damageBoostParams);
                break;


        }

        return null;
    }

   
}

