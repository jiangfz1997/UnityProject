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

               
        }

        return null;
    }

   
}

