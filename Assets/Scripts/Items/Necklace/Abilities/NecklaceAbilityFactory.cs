using UnityEngine;

public static class NecklaceAbilityFactory
{
    public static INecklaceAbility CreateAbility(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.AttackBoost:
                return new AttackSpeedAbility(0.15f); // ���5%����
            case AbilityType.SlowEnemy:
                return new SlowEnemyAbility(0.15f, 2f); // ���˼���10%������2��
            default:
                return null;
        }
    }
}