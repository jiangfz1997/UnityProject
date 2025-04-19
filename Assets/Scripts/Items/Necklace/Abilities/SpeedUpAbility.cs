using System;
using UnityEngine;

public class SpeedUpAbility : INecklaceAbility, IIdentifiableAbility
{
    private float speedUpPercent;
    private float speedUpDuration;
    private string abilityUID;


    public SpeedUpAbility(float percent, float duration=-9999f, string abilityUID=null)
    {
        speedUpPercent = percent;
        speedUpDuration = duration;
        this.abilityUID = abilityUID ?? Guid.NewGuid().ToString();
    }

    public void Apply(Player player)
    {
        Debug.Log($"Applying Speed Up Buff: {speedUpPercent}% for {speedUpDuration} seconds");
        Player.Instance.ApplyBuff(BuffType.SpeedUp, speedUpDuration, speedUpPercent, abilityUID); // -9999 means infinite duration
    }

    public void Remove(Player player)
    {
        Debug.Log($"Removing Speed Up Buff: {speedUpPercent}% for {speedUpDuration} seconds");
        Player.Instance.RemoveBuffBySource(abilityUID);
    }
    public void SetUID(string uid)
    {
        abilityUID = uid;
    }


}

