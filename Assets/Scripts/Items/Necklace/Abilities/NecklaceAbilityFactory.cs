using UnityEngine;

public static class NecklaceAbilityFactory
{
    public static INecklaceAbility CreateAbility(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.AttackBoost:
                return new AttackSpeedAbility(0.15f); // 提高5%攻速
            case AbilityType.SlowEnemy:
                return new SlowEnemyAbility(0.15f, 2f); // 敌人减速10%，持续2秒
            default:
                return null;
        }
    }
}