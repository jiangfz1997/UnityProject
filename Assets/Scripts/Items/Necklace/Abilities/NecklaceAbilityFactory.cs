using UnityEngine;

public static class NecklaceAbilityFactory
{
    public static INecklaceAbility CreateAbility(AbilityType type,string uid=null)
    {
        switch (type)
        {
            case AbilityType.AttackBoost:
                return new AttackSpeedAbility(0.15f, -9999, uid); 
            case AbilityType.SlowEnemy:
                return new SlowEnemyAbility(0.15f, -9999, uid); 
            case AbilityType.SpeedUp:
                return new SpeedUpAbility(0.05f, -9999, uid);
            case AbilityType.AttackUp:
                return new AttackUpAbility(0.05f, -9999, uid);
            case AbilityType.DefenseUp:
                return new DefenceUpAbility(0.3f, -9999, uid);
            case AbilityType.DoubleJump:
                return new DoubleJumpAbility(-9999, uid);
            default:
                return null;
        }
    }
}