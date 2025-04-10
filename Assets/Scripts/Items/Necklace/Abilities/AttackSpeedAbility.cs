using UnityEngine;

public class AttackSpeedAbility : INecklaceAbility
{
    private float bonusPercent;

    public AttackSpeedAbility(float percent)
    {
        bonusPercent = percent;
    }

    public void Apply(Player player)
    {
        player.ApplyBuff(BuffType.AttackSpeedUp, -9999,bonusPercent); // -9999 means infinite duration

    }

    public void Remove(Player player)
    {
        player.RemoveBuff(BuffType.AttackSpeedUp);
    }

}
