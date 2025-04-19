using System;
using UnityEngine;

public class AttackSpeedAbility : INecklaceAbility, IIdentifiableAbility
{
    private float bonusPercent;
    private float bonusDuration;
    private string abilityUID;
    public AttackSpeedAbility(float percent, float bonusDuration=-9999f, string uid = null)
    {
        bonusPercent = percent;
        this.bonusDuration = bonusDuration;
        this.abilityUID = uid ?? Guid.NewGuid().ToString();
    }

    public void Apply(Player player)
    {
        Debug.Log($"Applying Attack Speed Buff: {bonusPercent}% for {bonusDuration} seconds");
        player.ApplyBuff(BuffType.AttackSpeedUp, -9999, bonusPercent, abilityUID); // -9999 means infinite duration

    }

    public void Remove(Player player)
    {
        Debug.Log($"Removing Attack Speed Buff: {bonusPercent}% for {bonusDuration} seconds");
        player.RemoveBuffBySource(abilityUID);
    }
    public void SetUID(string uid)
    {
        abilityUID = uid;
    }

}
